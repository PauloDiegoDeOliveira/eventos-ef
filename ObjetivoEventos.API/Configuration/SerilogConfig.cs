using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;

namespace ObjetivoEventos.Application.Configuration
{
    public static class SerilogConfig
    {
        public static void AddSerilogApi()
        {
            LogConfiguration(GetConfiguration());
        }

        private static void LogConfiguration(IConfigurationRoot configurationBuilder)
        {
            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(configurationBuilder)
               .CreateLogger();
        }

        private static IConfigurationRoot GetConfiguration()
        {
            string ambiente = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddJsonFile($"appsettings.{ambiente}.json", optional: true)
                .Build();
        }
    }
}