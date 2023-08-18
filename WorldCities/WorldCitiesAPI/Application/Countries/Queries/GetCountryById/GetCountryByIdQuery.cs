using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;
using WorldCitiesAPI.Application.Cities.Queries.Dtos;
using WorldCitiesAPI.Application.Countries.Queries.Dtos;
using WorldCitiesAPI.Domain.CityAggregate;
using WorldCitiesAPI.Domain.CountryAggregate;
using WorldCitiesAPI.Resources;

namespace WorldCitiesAPI.Application.Countries.Queries.GetCountryById
{
    public class GetCountryByIdQuery : IRequest<Result<CountryDto>>
    {
        public int CountryId { get; set; }
    }

    public class GetCountryByIdQueryHandler : IRequestHandler<GetCountryByIdQuery, Result<CountryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetCountryByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<CountryDto>> Handle(GetCountryByIdQuery request, CancellationToken cancellationToken)
        {
            var country = await _context.Countries.FindAsync(request.CountryId);

            if (country is null)
                return Result.Failure<CountryDto>(Resource.CountryNotFound);

            return _mapper.Map<Country, CountryDto>(country);
        }
    }
}
