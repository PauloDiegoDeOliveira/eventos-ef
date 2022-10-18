using AutoMapper;
using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Application.Dtos.Mesa;
using ObjetivoEventos.Application.Dtos.Setor;
using ObjetivoEventos.Domain.Entitys;

namespace ObjetivoEventos.Application.Mappers
{
    public class SetorMappingProfile : Profile
    {
        public SetorMappingProfile()
        {
            Map();
        }

        private void Map()
        {
            CreateMap<PostSetorDto, Setor>().ReverseMap();
            CreateMap<PostSetorPadraoDto, Setor>().ReverseMap();
            CreateMap<PostAutomaticoSetorDto, Setor>().ReverseMap();

            CreateMap<PutSetorDto, Setor>().ReverseMap();
            CreateMap<PutAutomaticoSetorDto, Setor>().ReverseMap();
            CreateMap<ViewSetorDto, ViewSetorUsuarioAutenticadoDto>().ReverseMap();

            CreateMap<Setor, ViewSetorDto>().ReverseMap();

            CreateMap<Mesa, ReferenciaMesaDto>().ReverseMap();
            CreateMap<Cadeira, ReferenciaCadeiraDto>().ReverseMap();
        }
    }
}