using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Application.Cities.Queries.Dtos;
using WorldCitiesAPI.Application.Countries.Queries.Dtos;
using WorldCitiesAPI.Domain.CountryAggregate;

namespace WorldCitiesAPI.Application.Countries.Commands.UpdateCountry
{
    public class UpdateCountryCommand : IRequest<Result<CountryDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ISO2 { get; set; }
        public string ISO3 { get; set; }
    }

    public class UpdateCountryCommandHandler : IRequestHandler<UpdateCountryCommand, Result<CountryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UpdateCountryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<CountryDto>> Handle(UpdateCountryCommand request, CancellationToken cancellationToken)
        {
            Maybe<Country> maybeCountry = await _context.Countries.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (maybeCountry.HasNoValue)
                return Result.Failure<CountryDto>("Country Not Found");

            var updateResult = maybeCountry.Value.UpdateCountry(request.Name, request.ISO2, request.ISO3);

            if (updateResult.IsFailure)
                return Result.Failure<CountryDto>(updateResult.Error);

            var saveResult = await _context.SaveChangesAsyncWithValidation();

            if (saveResult.IsFailure)
                return Result.Failure<CountryDto>(saveResult.Error);

            return _mapper.Map<Country, CountryDto>(updateResult.Value);
        }
    }
}
