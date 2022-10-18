using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using ObjetivoEventos.Application.Applications;
using ObjetivoEventos.Application.BackgroundServices;
using ObjetivoEventos.Application.Extensions;
using ObjetivoEventos.Application.Hubs;
using ObjetivoEventos.Application.Interfaces;
using ObjetivoEventos.Domain.Core.Interfaces.Repositories;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Core.Notificacoes;
using ObjetivoEventos.Domain.Service;
using ObjetivoEventos.Infrastructure.Data.Repositorys;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ObjetivoEventos.Application.Configuration
{
    public static class DependencyInjectionConfig
    {
        public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
        {
            services.AddScoped<ISetorRepository, SetorRepository>();
            services.AddScoped<ISetorApplication, SetorApplication>();
            services.AddScoped<ISetorService, SetorService>();

            services.AddScoped<IMesaRepository, MesaRepository>();
            services.AddScoped<IMesaApplication, MesaApplication>();
            services.AddScoped<IMesaService, MesaService>();

            services.AddScoped<ICadeiraRepository, CadeiraRepository>();
            services.AddScoped<ICadeiraApplication, CadeiraApplication>();
            services.AddScoped<ICadeiraService, CadeiraService>();

            services.AddScoped<IEventoRepository, EventoRepository>();
            services.AddScoped<IEventoApplication, EventoApplication>();
            services.AddScoped<IEventoService, EventoService>();

            services.AddScoped<IReservaRepository, ReservaRepository>();
            services.AddScoped<IReservaApplication, ReservaApplication>();
            services.AddScoped<IReservaService, ReservaService>();

            services.AddScoped<ILocalRepository, LocalRepository>();
            services.AddScoped<ILocalApplication, LocalApplication>();
            services.AddScoped<ILocalService, LocalService>();

            services.AddScoped<IUsuarioApplication, UsuarioApplication>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<IUsuarioService, UsuarioService>();

            services.AddScoped<IPedidoApplication, PedidoApplication>();
            services.AddScoped<IPedidoRepository, PedidoRepository>();
            services.AddScoped<IPedidoService, PedidoService>();

            services.AddHostedService<MyBackgroundService>();
            services.AddSingleton<ReservaBackgroundService>();
            services.AddSingleton<PedidoBackgroundService>();
            services.AddSingleton<EventoBackgroundService>();

            services.AddSingleton<MessageHub>();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddScoped<IUser, AspNetUser>();

            services.AddScoped<IEmailApplication, EmailApplication>();

            services.AddScoped<INotificador, Notificador>();

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        }
    }
}