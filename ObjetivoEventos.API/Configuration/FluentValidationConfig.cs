using FluentValidation;
using FluentValidation.AspNetCore;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using ObjetivoEventos.Application.Validations.Cadeira;
using ObjetivoEventos.Application.Validations.Evento;
using ObjetivoEventos.Application.Validations.Local;
using ObjetivoEventos.Application.Validations.Mesa;
using ObjetivoEventos.Application.Validations.Pedido;
using ObjetivoEventos.Application.Validations.Reserva;
using ObjetivoEventos.Application.Validations.Setor;
using ObjetivoEventos.Application.Validations.Usuario;
using System.Globalization;
using System.Text.Json.Serialization;

namespace ObjetivoEventos.Application.Configuration
{
    public static class FluentValidationConfig
    {
        public static void AddFluentValidationConfiguration(this IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(config =>
                {
                    config.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    config.SerializerSettings.Converters.Add(new StringEnumConverter());
                })
                .AddJsonOptions(conf =>
                {
                    conf.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });

            services.AddFluentValidationAutoValidation();

            services.AddValidatorsFromAssemblyContaining<PostReservaValidator>();
            services.AddValidatorsFromAssemblyContaining<PutReservaValidator>();
            services.AddValidatorsFromAssemblyContaining<PutSituacaoReservaValidator>();
            services.AddValidatorsFromAssemblyContaining<PutStatusReservaValidator>();

            services.AddValidatorsFromAssemblyContaining<PostCadeiraValidator>();
            services.AddValidatorsFromAssemblyContaining<PutCadeiraValidator>();
            services.AddValidatorsFromAssemblyContaining<PutStatusCadeiraValidator>();

            services.AddValidatorsFromAssemblyContaining<PostLocalValidator>();
            services.AddValidatorsFromAssemblyContaining<PutLocalValidator>();
            services.AddValidatorsFromAssemblyContaining<PutStatusLocalValidator>();

            services.AddValidatorsFromAssemblyContaining<PostEventoValidator>();
            services.AddValidatorsFromAssemblyContaining<PutEventoValidator>();
            services.AddValidatorsFromAssemblyContaining<PutStatusEventoValidator>();

            services.AddValidatorsFromAssemblyContaining<PostMesaValidator>();
            services.AddValidatorsFromAssemblyContaining<PutMesaValidator>();
            services.AddValidatorsFromAssemblyContaining<PutStatusMesaValidator>();

            services.AddValidatorsFromAssemblyContaining<PostSetorValidator>();
            services.AddValidatorsFromAssemblyContaining<PutSetorValidator>();
            services.AddValidatorsFromAssemblyContaining<PutStatusSetorValidator>();

            services.AddValidatorsFromAssemblyContaining<PostPedidoValidator>();
            services.AddValidatorsFromAssemblyContaining<PutPedidoValidator>();
            services.AddValidatorsFromAssemblyContaining<PutStatusPedidoValidator>();
            services.AddValidatorsFromAssemblyContaining<PostGerarQRCodeValidator>();
            services.AddValidatorsFromAssemblyContaining<PutPedidoTrocaValidator>();
            services.AddValidatorsFromAssemblyContaining<PutSituacaoPedidoValidator>();

            services.AddValidatorsFromAssemblyContaining<PostUsuarioValidator>();
            // services.AddValidatorsFromAssemblyContaining<PutUsuarioValidator>();

            ValidatorOptions.Global.LanguageManager.Culture = new CultureInfo("pt-BR");

            services.AddFluentValidationRulesToSwagger();
        }
    }
}