using AutoMapper;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Application.Cities.Queries.Dtos;
using WorldCitiesAPI.Domain.CityAggregate;
using WorldCitiesAPI.Domain.CountryAggregate;

namespace WorldCitiesAPI.Application.Cities.Commands.CreateCity
{
    public class CreateCityCommand : IRequest<Result<CityDto>>
    {
        public string Name { get; set; } = null!;
        public decimal lat { get; set; }
        public decimal Lon { get; set; }
        public int CountryId { get; set; }
    }

    public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, Result<CityDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CreateCityCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<CityDto>> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            Maybe<Country> maybeCountry = await _context.Countries.FirstOrDefaultAsync(x => x.Id == request.CountryId);

            var createResult = City.CreateCity(
                request.Name,
                request.lat,
                request.Lon,
                maybeCountry);

            if (createResult.IsFailure)
                return Result.Failure<CityDto>(createResult.Error);

            _context.Cities.Add(createResult.Value);

            var saveResult = await _context.SaveChangesAsyncWithValidation();

            if (saveResult.IsFailure)
                return Result.Failure<CityDto>(saveResult.Error);

            return _mapper.Map<City, CityDto>(createResult.Value);
        }
    }
}
