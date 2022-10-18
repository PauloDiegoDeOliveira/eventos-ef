using Microsoft.Extensions.DependencyInjection;
using ObjetivoEventos.Application.Mappers;

namespace ObjetivoEventos.Application.Configuration
{
    public static class AutoMapperConfig
    {
        public static void AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(
               typeof(SetorMappingProfile),
               typeof(MesaMappingProfile),
               typeof(CadeiraMappingProfile),
               typeof(ReservaMappingProfile),
               typeof(EventoMappingProfile),
               typeof(LocalMappingProfile),
               typeof(UsuarioMappingProfile));
        }
    }
}