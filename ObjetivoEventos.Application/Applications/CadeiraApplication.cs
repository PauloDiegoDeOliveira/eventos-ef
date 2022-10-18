using AutoMapper;
using ObjetivoEventos.Application.Applications.Base;
using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Interfaces;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Applications
{
    public class CadeiraApplication : ApplicationBase<Cadeira, ViewCadeiraDto, PostCadeiraDto, PutCadeiraDto, PutStatusDto>, ICadeiraApplication
    {
        private readonly ICadeiraService serviceCadeira;

        public CadeiraApplication(ICadeiraService serviceCadeira,
                                INotificador notificador,
                                IMapper mapper) : base(serviceCadeira, notificador, mapper)
        {
            this.serviceCadeira = serviceCadeira;
        }

        public async Task<ViewPagedListDto<Cadeira, ViewCadeiraDto>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            PagedList<Cadeira> pagedList = await serviceCadeira.GetPaginationAsync(parametersPalavraChave);
            return new ViewPagedListDto<Cadeira, ViewCadeiraDto>(pagedList, mapper.Map<List<ViewCadeiraDto>>(pagedList));
        }

        public async Task<List<ViewCadeiraDto>> PostAutomaticoAsync(List<PostCadeiraDto> postCadeiraDtos)
        {
            return mapper.Map<List<ViewCadeiraDto>>(await serviceCadeira.PostAutomaticoAsync(mapper.Map<List<Cadeira>>(postCadeiraDtos)));
        }

        public bool ValidarId(Guid id)
        {
            return serviceCadeira.ValidarId(id);
        }

        public bool ValidarFileiraEColunaPost(PostCadeiraDto postCadeiraDto)
        {
            return mapper.Map<bool>(serviceCadeira.ValidarFileiraEColuna(mapper.Map<Cadeira>(postCadeiraDto)));
        }

        public bool ValidarFileiraEColunaPut(PutCadeiraDto putCadeiraDto)
        {
            return mapper.Map<bool>(serviceCadeira.ValidarFileiraEColuna(mapper.Map<Cadeira>(putCadeiraDto)));
        }
    }
}