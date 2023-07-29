using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Application.Cities.Queries.Dtos;
using WorldCitiesAPI.Domain.CityAggregate;
using WorldCitiesAPI.Domain.CountryAggregate;

namespace WorldCitiesAPI.Application.Cities.Commands.UpdateCity
{
    public class UpdateCityCommand : IRequest<Result<CityDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal lat { get; set; }
        public decimal Lon { get; set; }
        public decimal CountryId { get; set; }
    }

    public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, Result<CityDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public UpdateCityCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<CityDto>> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            Maybe<Country> maybeCountry = await _context.Countries.FirstOrDefaultAsync(x => x.Id == request.CountryId);

            Maybe<City> maybeCity = await _context.Cities.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (maybeCity.HasNoValue)
                return Result.Failure<CityDto>("City Not Found");

            var updateResult = maybeCity.Value.UpdateCity(request.Name, request.lat, request.Lon, maybeCountry);

            if (updateResult.IsFailure)
                return Result.Failure<CityDto>(updateResult.Error);

            var saveResult = await _context.SaveChangesAsyncWithValidation();

            if (saveResult.IsFailure)
                return Result.Failure<CityDto>(saveResult.Error);

            return _mapper.Map<City, CityDto>(updateResult.Value);
        }
    }
}
