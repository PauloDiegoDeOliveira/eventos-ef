using AutoMapper;
using ObjetivoEventos.Application.Dtos.Local;
using ObjetivoEventos.Domain.Entitys;

namespace ObjetivoEventos.Application.Mappers
{
    public class LocalMappingProfile : Profile
    {
        public LocalMappingProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<Local, ViewLocalDto>().ReverseMap();
            CreateMap<Local, PostLocalDto>().ReverseMap();
            CreateMap<Local, PutLocalDto>().ReverseMap();
            CreateMap<Local, ViewLocalDetalhesDto>().ReverseMap();
        }
    }
}