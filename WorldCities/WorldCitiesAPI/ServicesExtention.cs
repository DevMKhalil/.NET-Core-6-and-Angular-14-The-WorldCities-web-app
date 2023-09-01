using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Application;
using WorldCitiesAPI.Data;
using Microsoft.AspNetCore.Identity;
using WorldCitiesAPI.Domain.ApplicationUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

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
                //builder.AddFilter(DbLoggerCategory.Database.Command.Name, LogLevel.Information);
            });

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options
                .UseLoggerFactory(ConsolLoggerFactory)
                .EnableSensitiveDataLogging()
                .UseSqlServer(configration.GetConnectionString("DefaultConnection"));

            });

            // Add ASP.NET Core Identity support
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true; 
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true; 
                options.Password.RequiredLength = 8;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();

            // Add Authentication services & middlewares
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; 
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configration["JwtSettings:Issuer"],
                    ValidAudience = configration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configration["JwtSettings:SecurityKey"]))
                };
            });

            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
        }
    }
}
