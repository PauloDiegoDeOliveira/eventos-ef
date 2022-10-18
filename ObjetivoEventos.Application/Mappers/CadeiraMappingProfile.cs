using AutoMapper;
using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Domain.Entitys;

namespace ObjetivoEventos.Application.Mappers
{
    public class CadeiraMappingProfile : Profile
    {
        public CadeiraMappingProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<PostCadeiraDto, Cadeira>().ReverseMap();
            CreateMap<PutCadeiraDto, Cadeira>().ReverseMap();
            CreateMap<Cadeira, ViewCadeiraDto>().ReverseMap();
        }
    }
}