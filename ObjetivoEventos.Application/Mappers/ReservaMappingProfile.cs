using AutoMapper;
using ObjetivoEventos.Application.Dtos.Reserva;
using ObjetivoEventos.Domain.Entitys;

namespace ObjetivoEventos.Application.Mappers
{
    public class ReservaMappingProfile : Profile
    {
        public ReservaMappingProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<Reserva, ViewReservaDto>().ReverseMap();
            CreateMap<Reserva, PostReservaDto>().ReverseMap();
            CreateMap<Reserva, PutReservaDto>().ReverseMap();
        }
    }
}