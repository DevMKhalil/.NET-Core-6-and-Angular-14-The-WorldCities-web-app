using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Application.Cities.Queries.Dtos;
using WorldCitiesAPI.Domain.CityAggregate;

namespace WorldCitiesAPI.Application.Cities.Queries.GetCities
{
    public class GetCitiesQuery : IRequest<List<CityDto>>
    {
    }

    public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, List<CityDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetCitiesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<List<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Cities
                .ProjectTo<CityDto>(_mapper.ConfigurationProvider).ToListAsync();
        }
    }
}
