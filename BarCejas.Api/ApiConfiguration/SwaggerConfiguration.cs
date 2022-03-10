using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace BarCejas.Api.ApiConfiguration
{
    public static class SwaggerConfiguration
    {
        /// <summary>
        ///     <para>Bar de cejas API v1.</para>
        /// </summary>
        private const string EndpointName = "Bar de cejas API v1";

        /// <summary>
        ///     <para>/swagger/v1/swagger.json.</para>
        /// </summary>
        private const string EndpointUrl = "/swagger/v1/swagger.json";

        /// <summary>
        ///     <para>v1.</para>
        /// </summary>
        private const string DocNameV2 = "v1";

        /// <summary>
        ///     <para>Bar de cejas API.</para>
        /// </summary>
        private const string DocInfoTitle = "Bar de cejas API";

        /// <summary>
        ///     <para>v1.</para>
        /// </summary>
        private const string DocInfoVersion = "v1";

        /// <summary>
        ///     <para>Bar de cejas API in ASP.NET Core 3.1.</para>
        /// </summary>
        private const string DocInfoDescription = "Bar de cejas Api";

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            _ = services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc(
                    DocNameV2,
                    new OpenApiInfo
                    {
                        Title = DocInfoTitle,
                        Version = DocInfoVersion,
                        Description = DocInfoDescription,
                    });
            });
        }

        public static void UseSwaggerUi(this IApplicationBuilder app)
        {
            _ = app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            _ = app.UseSwaggerUI(config => config.SwaggerEndpoint(EndpointUrl, EndpointName));
        }
    }
}
