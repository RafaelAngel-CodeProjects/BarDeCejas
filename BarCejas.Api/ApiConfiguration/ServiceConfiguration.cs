using BarCejas.Core.CustomEntities;
using BarCejas.Core.Interfaces;
using BarCejas.Core.Services;
using BarCejas.Infrastructure.Data;
using BarCejas.Infrastructure.Interfaces;
using BarCejas.Infrastructure.Options;
using BarCejas.Infrastructure.Repositories;
using BarCejas.Infrastructure.Services;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.Globalization;

namespace BarCejas.Api.ApiConfiguration
{
    public static class ServiceConfiguration
    {
        private const string AllowSpecificOrigins = "AllowAllOrigin";

        internal static void ConfigureDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            _ = services.AddAutoMapper(typeof(Startup));
            _ = services.AddDbContext<BarCejasContext>(options => options.UseSqlServer(configuration.GetConnectionString("BarCejasContext")));

            _ = services.Configure<PaginationOptions>(configuration.GetSection("Pagination"));
            _ = services.Configure<PasswordOptions>(configuration.GetSection("PasswordOptions"));


            _ = services.AddTransient<IUserService, UserService>();
            
            _ = services.AddScoped(typeof(IRepository<>), typeof(BaseRepository<>));
            
            _ = services.AddTransient<IUnitOfWork, UnitOfWork>();
            
            _ = services.AddSingleton<IPasswordHasher, PasswordService>();
            
            _ = services.AddSingleton<IUriService>(provider =>
            {
                var accesor = provider.GetRequiredService<IHttpContextAccessor>();
                var request = accesor.HttpContext.Request;
                var absoluteUri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(absoluteUri);
            });

        }

        internal static void ConfigureLocalization(this IServiceCollection services)
        {
            _ = services.AddLocalization(opt => opt.ResourcesPath = "Resources");
            _ = services.Configure<RequestLocalizationOptions>(options =>
            {
                List<CultureInfo> supportedCultures = new List<CultureInfo>();
                options.DefaultRequestCulture = new RequestCulture("es");
                options.SupportedCultures = supportedCultures;
            });
        }

        internal static void UseExceptionMiddleware(this IApplicationBuilder app) => app.UseMiddleware(typeof(ExceptionHandler));

        internal static void UseLocalization(this IApplicationBuilder app)
        {
            IOptions<RequestLocalizationOptions> localizedOptions = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            _ = app.UseRequestLocalization(localizedOptions.Value);
        }
        internal static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(CreateJwtConfigureOptions(configuration));

        }

        private static Action<JwtBearerOptions> CreateJwtConfigureOptions(IConfiguration configuration)
        {
            return options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Authenticarion:Issuer"],
                    ValidAudience = configuration["Authenticarion:Audience"],
                    //IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                };
            };
        }
        internal static void ConfigureCors(this IServiceCollection services)
        {
            _ = services.AddCors(options => options.AddPolicy(
                AllowSpecificOrigins,
                builder =>
                {
                    _ = builder.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                }));
        }

        internal static void UseCorsDev(this IApplicationBuilder app) => app.UseCors(AllowSpecificOrigins);
    }
}
