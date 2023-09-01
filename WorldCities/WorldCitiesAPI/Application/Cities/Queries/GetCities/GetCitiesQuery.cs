using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Application.Cities.Queries.Dtos;
using WorldCitiesAPI.Common;
using WorldCitiesAPI.Common.Helper;
using WorldCitiesAPI.Domain.CityAggregate;

namespace WorldCitiesAPI.Application.Cities.Queries.GetCities
{
    public class GetCitiesQuery : IRequest<ApiResult<CityDto>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = SharedConst.defaultPageSize;
        public string? SortColumn { get; set; } = null;
        public string? SortOrder { get; set; } = null;
        public string? FilterColumn { get; set; } = null;
        public string? FilterQuery { get; set; } = null;
    }

    public class GetCitiesQueryHandler : IRequestHandler<GetCitiesQuery, ApiResult<CityDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetCitiesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ApiResult<CityDto>> Handle(GetCitiesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Cities
                .Select(x => new CityDto
                {
                    Name = x.Name,
                    CountryId = x.CountryId,
                    Id = x.Id,
                    lat = x.Lat,
                    Lon = x.Lon,
                    CountryName = x.Country!.Name
                })
                .AsNoTracking()
                //.ProjectTo<CityDto>(_mapper.ConfigurationProvider)
                ;

            return await ApiResult<CityDto>
                .CreateAsync(
                query,
                request.PageIndex,
                request.PageSize,
                request.SortColumn,
                request.SortOrder,
                request.FilterColumn,
                request.FilterQuery);
        }
    }
}
