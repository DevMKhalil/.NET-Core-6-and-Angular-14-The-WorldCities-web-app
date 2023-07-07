using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Application;
using WorldCitiesAPI.Data;

namespace WorldCitiesAPI
{
    public static class ServicesExtention
    {
        public static void AddContext(this IServiceCollection services, IConfiguration configration)
        {
            ILoggerFactory ConsolLoggerFactory = LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
                builder.AddFilter(DbLoggerCategory.Database.Transaction.Name, LogLevel.Debug);
                builder.AddFilter(DbLoggerCategory.Database.Connection.Name, LogLevel.Information);
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                .UseLoggerFactory(ConsolLoggerFactory)
                .EnableSensitiveDataLogging()
                .UseSqlServer(configration.GetConnectionString("DefaultConnection"));

            });

            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
        }
    }
}
