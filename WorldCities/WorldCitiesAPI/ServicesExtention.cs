using Microsoft.EntityFrameworkCore;
using WorldCitiesAPI.Application;
using WorldCitiesAPI.Data;
using Microsoft.AspNetCore.Identity;
using WorldCitiesAPI.Domain.ApplicationUser;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Cors;

namespace WorldCitiesAPI
{
    public static class ServicesExtention
    {
        public static void AddContext(this IServiceCollection services, IConfiguration configration)
        {
            //add Swagger with authentication JWT
            services.AddSwaggerGen(
                c =>
                {
                    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                    {
                        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                        Description = "Please Insert Token",
                        Name = "Authorization",
                        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "bearer"
                    });
                    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[]{ }
                    }
                });
                }
            );

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
                    ValidateLifetime = true,
                    ValidateIssuer = true,
                    ValidIssuer = configration["JwtSettings:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = configration["JwtSettings:Audience"],
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configration["JwtSettings:SecurityKey"]))
                };
            });

            services.AddCors(options =>
                options.AddPolicy(name: "AngularPolicy",
                    cfg => {
                        cfg.AllowAnyHeader();
                        cfg.AllowAnyMethod();
                        cfg.WithOrigins(configration["AllowedCORS"]);
                    }));

            services.AddTransient<IApplicationDbContext, ApplicationDbContext>();
        }
    }
}
