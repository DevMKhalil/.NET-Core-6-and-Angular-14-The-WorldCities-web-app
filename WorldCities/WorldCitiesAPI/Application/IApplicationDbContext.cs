using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Domain.CityAggregate;
using WorldCitiesAPI.Domain.CountryAggregate;

namespace WorldCitiesAPI.Application
{
    public interface IApplicationDbContext
    {
        public DbSet<City> Cities { get; }
        public DbSet<Country> Countries { get; }

        Task<Result> SaveChangesAsyncWithValidation(CancellationToken cancellationToken = default(CancellationToken));
    }
}
