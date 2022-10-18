using AutoMapper;
using ObjetivoEventos.Application.Dtos.Evento;
using ObjetivoEventos.Application.Dtos.Local;
using ObjetivoEventos.Domain.Entitys;

namespace ObjetivoEventos.Application.Mappers
{
    public class EventoMappingProfile : Profile
    {
        public EventoMappingProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<Evento, ViewEventoDto>().ReverseMap();
            CreateMap<Evento, PostEventoDto>().ReverseMap();
            CreateMap<Evento, PutEventoDto>().ReverseMap();
            CreateMap<Evento, ViewEventoUsuarioAutenticadoDto>().ReverseMap();
            CreateMap<Evento, ViewEventoDetalhesDto>().ReverseMap();

            CreateMap<Local, ViewLocalDto>().ReverseMap();
        }
    }
}