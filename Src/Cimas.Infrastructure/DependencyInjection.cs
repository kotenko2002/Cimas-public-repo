using Cimas.Application.Interfaces;
using Cimas.Domain.Entities.Users;
using Cimas.Infrastructure.Auth;
using Cimas.Infrastructure.Common;
using Cimas.Infrastructure.Repositories;
using Cimas.Infrastructure.Services.FileStorage;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Cimas.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, ConfigurationManager configuration)
        {
            services
                .AddDatabaseServices(configuration)
                .AddAuthScheme(configuration);

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services
                .Configure<JwtConfig>(configuration.GetSection(JwtConfig.Section))
                .AddScoped<IJwtTokensService, JwtTokensService>();

            services.AddGoogleDrive(configuration);
            services.AddScoped<IFileStorageService, GoogleDriveService>();

            return services;
        }

        private static IServiceCollection AddDatabaseServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<User, IdentityRole<Guid>>()
                .AddEntityFrameworkStores<CimasDbContext>()
                .AddDefaultTokenProviders()
                .AddErrorDescriber<UkrainianIdentityErrorDescriber>();

            services.AddDbContext<CimasDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        private static IServiceCollection AddAuthScheme(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero,

                    ValidAudience = configuration["Jwt:ValidAudience"],
                    ValidIssuer = configuration["Jwt:ValidIssuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Secret"]))
                };
            });

            return services;
        }

        private static IServiceCollection AddGoogleDrive(this IServiceCollection services, ConfigurationManager configuration)
        {
            services.Configure<GoogleDriveConfig>(configuration.GetSection("GoogleDrive"));
            GoogleDriveConfig config = services.BuildServiceProvider()
                .GetRequiredService<IOptionsSnapshot<GoogleDriveConfig>>().Value;

            services.AddSingleton(provider =>
            {
                ServiceAccountCredential credential;

                using (var stream = new FileStream("google_drive_credentials.json", FileMode.Open, FileAccess.Read))
                    credential = new ServiceAccountCredential(new ServiceAccountCredential.Initializer(config.ClientEmail)
                    {
                        Scopes = new string[] { DriveService.Scope.Drive, DriveService.Scope.DriveFile }
                    }.FromPrivateKey(config.PrivateKey));

                return new DriveService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = config.ApplicationName
                });
            });

            return services;
        }
    }
}
