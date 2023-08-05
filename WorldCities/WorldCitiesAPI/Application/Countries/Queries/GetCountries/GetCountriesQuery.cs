using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Application.Cities.Queries.Dtos;
using WorldCitiesAPI.Application.Countries.Queries.Dtos;
using WorldCitiesAPI.Common.Helper;
using WorldCitiesAPI.Data;

namespace WorldCitiesAPI.Application.Countries.Queries.GetCountries
{
    public class GetCountriesQuery : IRequest<ApiResult<CountryDto>>
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; } = SharedConst.defaultPageSize;
        public string? SortColumn { get; set; } = null;
        public string? SortOrder { get; set; } = null;
        public string? FilterColumn { get; set; } = null;
        public string? FilterQuery { get; set; } = null;
    }

    public class GetCountriesQueryHandler : IRequestHandler<GetCountriesQuery, ApiResult<CountryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetCountriesQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ApiResult<CountryDto>> Handle(GetCountriesQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Countries
                .AsNoTracking()
                .ProjectTo<CountryDto>(_mapper.ConfigurationProvider);

            return await ApiResult<CountryDto>
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
