using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShareMyCalendar.Authentication.Data;
using ShareMyCalendar.Authentication.Services;
using ShareMyCalendar.Authentication.Settings;
using System.Text;
using System.Text.Json.Serialization;

namespace ShareMyCalendar.Authentication.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddShareMyCalendar(this IServiceCollection services, AppSettings appSettings)
        {
            if (string.IsNullOrEmpty(appSettings.ConnectionString))
            {
                services.AddDbContext<DatabaseContext>(optionsBuilder =>
                {
                    optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
                });
            }
            else
            {
                services.AddDbContext<DatabaseContext>(optionsBuilder =>
                {
                    optionsBuilder.UseSqlServer(appSettings.ConnectionString);
                });
            }

            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

            services
                .AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                });
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = appSettings.Jwt.Issuer,
                        ValidAudience = appSettings.Jwt.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(appSettings.Jwt.Key)),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateIssuerSigningKey = true,
                    };
                });

            services.AddAuthorization();

            services
                .AddIdentity<User, Role>(options =>
                {
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IAccessTokenGenerator, AccessTokenGenerator>();
            services.AddScoped(serviceProvider => appSettings);
            services.AddScoped(serviceProvider => appSettings.Jwt);
        }
    }
}
