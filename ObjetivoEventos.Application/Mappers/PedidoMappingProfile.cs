using AutoMapper;
using ObjetivoEventos.Application.Dtos.Pedido;
using ObjetivoEventos.Application.Dtos.Reserva;
using ObjetivoEventos.Domain.Entitys;

namespace ObjetivoEventos.Application.Mappers
{
    public class PedidoMappingProfile : Profile
    {
        public PedidoMappingProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<Pedido, ViewPedidoDto>().ReverseMap();
            CreateMap<Pedido, PostPedidoDto>().ReverseMap();
            CreateMap<Pedido, PutPedidoDto>().ReverseMap();

            CreateMap<Reserva, ReferenciaReservaDto>().ReverseMap();
        }
    }
}