using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using WorldCitiesAPI.Application.Cities.Queries.Dtos;
using WorldCitiesAPI.Application.Countries.Queries.Dtos;
using WorldCitiesAPI.Domain.CityAggregate;
using WorldCitiesAPI.Domain.CountryAggregate;

namespace WorldCitiesAPI.Application.Countries.Commands.CreateCountry
{
    public class CreateCountryCommand : IRequest<Result<CountryDto>>
    {
        public string Name { get; set; }
        public string ISO2 { get; set; }
        public string ISO3 { get; set; }
    }

    public class CreateCountryCommandHandler : IRequestHandler<CreateCountryCommand, Result<CountryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public CreateCountryCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<CountryDto>> Handle(CreateCountryCommand request, CancellationToken cancellationToken)
        {
            var createResult = Country.CreateCountry(request.Name, request.ISO2, request.ISO3);

            if (createResult.IsFailure)
                return Result.Failure<CountryDto>(createResult.Error);

            _context.Countries.Add(createResult.Value);

            var saveResult = await _context.SaveChangesAsyncWithValidation();

            if (saveResult.IsFailure)
                return Result.Failure<CountryDto>(saveResult.Error);

            return _mapper.Map<Country, CountryDto>(createResult.Value);
        }
    }
}
