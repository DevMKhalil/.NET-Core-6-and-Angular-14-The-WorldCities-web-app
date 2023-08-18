using AutoMapper;
using CSharpFunctionalExtensions;
using CSharpFunctionalExtensions.ValueTasks;
using MediatR;
using WorldCitiesAPI.Application.Cities.Queries.Dtos;
using WorldCitiesAPI.Domain.CityAggregate;
using WorldCitiesAPI.Resources;

namespace WorldCitiesAPI.Application.Cities.Queries.GetCityById
{
    public class GetCityByIdQuery : IRequest<Result<CityDto>>
    {
        public int CityId { get; set; }
    }

    public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, Result<CityDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        public GetCityByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<Result<CityDto>> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            var city = await _context.Cities.FindAsync(request.CityId);

            if (city is null)
                return Result.Failure<CityDto>(Resource.CityNotFound);

            return _mapper.Map<City, CityDto>(city);
        }
    }
}
