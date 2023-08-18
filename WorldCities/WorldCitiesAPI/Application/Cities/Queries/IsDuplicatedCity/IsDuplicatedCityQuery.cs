using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using WorldCitiesAPI.Domain.CityAggregate;
using WorldCitiesAPI.Domain.CountryAggregate;

namespace WorldCitiesAPI.Application.Cities.Queries.IsDuplicatedCity
{
    public class IsDuplicatedCityQuery : IRequest<bool>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Lat { get; set; }
        public decimal Lon { get; set; }
        public int CountryId { get; set; }
    }

    public class IsDuplicatedCityQueryHandler : IRequestHandler<IsDuplicatedCityQuery, bool>
    {
        private readonly IApplicationDbContext _context;
        public IsDuplicatedCityQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> Handle(IsDuplicatedCityQuery request, CancellationToken cancellationToken)
        {
            return await _context.Cities.AnyAsync(e => 
                e.Name == request.Name 
                && e.Lat == request.Lat
                && e.Lon == request.Lon
                && e.CountryId == request.CountryId 
                && e.Id != request.Id);
        }
    }
}
