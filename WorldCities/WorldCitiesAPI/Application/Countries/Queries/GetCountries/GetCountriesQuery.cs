using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Application.Countries.Queries.Dtos;

namespace WorldCitiesAPI.Application.Countries.Queries.GetCountries
{
    public class GetCountriesQuery : IRequest<List<CountryDto>>
    {
    }

    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, List<CountryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetCountriesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CountryDto>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Countries
                .ProjectTo<CountryDto>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
