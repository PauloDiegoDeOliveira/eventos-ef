using AutoMapper;
using ObjetivoEventos.Application.Dtos.Identity.Usuario;
using ObjetivoEventos.Identity.Entitys;

namespace ObjetivoEventos.Application.Mappers
{
    public class UsuarioMappingProfile : Profile
    {
        public UsuarioMappingProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<Usuario, ViewUsuarioDto>().ReverseMap();
        }
    }
}