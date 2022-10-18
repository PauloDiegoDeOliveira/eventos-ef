using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.DependencyInjection;

namespace ObjetivoEventos.Application.Configuration
{
    public static class VersionConfig
    {
        public static IServiceCollection AddVersionConfiguration(this IServiceCollection services)
        {
            services.AddApiVersioning(o =>
            {
                o.UseApiBehavior = false;
                o.ReportApiVersions = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.ApiVersionReader = ApiVersionReader.Combine(
                    //new HeaderApiVersionReader("x-api-version"),
                    //new QueryStringApiVersionReader(),
                    new UrlSegmentApiVersionReader());
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'V'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            return services;
        }
    }
}