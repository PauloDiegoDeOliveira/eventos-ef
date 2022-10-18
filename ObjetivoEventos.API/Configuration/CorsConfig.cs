using Microsoft.Extensions.DependencyInjection;

namespace ObjetivoEventos.Application.Configuration
{
    public static class CorsConfig
    {
        public static void AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Development", builder =>
                {
                    builder.AllowAnyMethod()
                          .AllowAnyHeader()
                          .SetIsOriginAllowed(origin => true)
                          .AllowCredentials();
                });

                options.AddPolicy("Production", builder =>
                {
                    builder.AllowAnyMethod()
                           .AllowAnyHeader()
                           .SetIsOriginAllowed(origin => true)
                           .AllowCredentials();
                });

                options.AddPolicy("Staging", builder =>
                {
                    builder.AllowAnyMethod()
                           .AllowAnyHeader()
                           .SetIsOriginAllowed(origin => true)
                           .AllowCredentials();
                });
            });
        }
    }
}