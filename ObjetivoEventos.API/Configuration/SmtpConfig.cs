using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ObjetivoEventos.Application.Dtos.SMTP;

namespace ObjetivoEventos.Application.Configuration
{
    public static class SmtpConfig
    {
        public static void AddSMTPConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<SmtpOptions>(configuration.GetSection("SMTPOptions"));
        }
    }
}