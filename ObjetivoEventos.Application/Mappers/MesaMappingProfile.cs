using AutoMapper;
using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Application.Dtos.Mesa;
using ObjetivoEventos.Domain.Entitys;

namespace ObjetivoEventos.Application.Mappers
{
    public class MesaMappingProfile : Profile
    {
        public MesaMappingProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<PostMesaDto, Mesa>().ReverseMap();
            CreateMap<PutMesaDto, Mesa>().ReverseMap();
            CreateMap<Mesa, ViewMesaDto>().ReverseMap();
            CreateMap<Mesa, ViewMesaUsuarioAutenticadoDto>().ReverseMap();
            CreateMap<ViewMesaDto, ViewMesaUsuarioAutenticadoDto>().ReverseMap();

            CreateMap<Cadeira, ReferenciaCadeiraDto>().ReverseMap();
        }
    }
}