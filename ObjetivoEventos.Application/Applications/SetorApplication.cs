using AutoMapper;
using ObjetivoEventos.Application.Applications.Base;
using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Dtos.Setor;
using ObjetivoEventos.Application.Interfaces;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Applications
{
    public class SetorApplication : ApplicationBase<Setor, ViewSetorDto, PostSetorDto, PutSetorDto, PutStatusDto>, ISetorApplication
    {
        private readonly ISetorService serviceSetor;

        public SetorApplication(ISetorService serviceSetor, INotificador notificador,
                                IMapper mapper) : base(serviceSetor, notificador, mapper)
        {
            this.serviceSetor = serviceSetor;
        }

        public async Task<ViewPagedListDto<Setor, ViewSetorDto>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            PagedList<Setor> pagedList = await serviceSetor.GetPaginationAsync(parametersPalavraChave);
            return new ViewPagedListDto<Setor, ViewSetorDto>(pagedList, mapper.Map<List<ViewSetorDto>>(pagedList));
        }

        public async Task<ViewSetorDto> PostSetorByCadeirasAutomaticoAsync(PostAutomaticoSetorDto postAutomaticoSetorDto)
        {
            return mapper.Map<ViewSetorDto>(await serviceSetor.PostSetorByCadeirasAutomaticoAsync(mapper.Map<Setor>(postAutomaticoSetorDto), postAutomaticoSetorDto.GridParameters));
        }

        public async Task<ViewSetorDto> PutSetorByCadeirasAutomaticoAsync(PutAutomaticoSetorDto putAutomaticoSetorDto)
        {
            return mapper.Map<ViewSetorDto>(await serviceSetor.PutSetorByCadeirasAutomaticoAsync(mapper.Map<Setor>(putAutomaticoSetorDto), putAutomaticoSetorDto.GridParameters));
        }

        public bool ValidarId(Guid id)
        {
            return serviceSetor.ValidarId(id);
        }
    }
}