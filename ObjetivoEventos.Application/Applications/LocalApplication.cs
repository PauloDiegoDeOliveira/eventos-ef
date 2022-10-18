using AutoMapper;
using ObjetivoEventos.Application.Applications.Base;
using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Local;
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
    public class LocalApplication : ApplicationBase<Local, ViewLocalDto, PostLocalDto, PutLocalDto, PutStatusDto>, ILocalApplication
    {
        private readonly ILocalService localService;

        public LocalApplication(ILocalService localService,
                                INotificador notificador,
                                IMapper mapper) : base(localService, notificador, mapper)
        {
            this.localService = localService;
        }

        public async Task<ViewPagedListDto<Local, ViewLocalDto>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            PagedList<Local> pagedList = await localService.GetPaginationAsync(parametersPalavraChave);
            return new ViewPagedListDto<Local, ViewLocalDto>(pagedList, mapper.Map<List<ViewLocalDto>>(pagedList));
        }

        public bool ValidarId(Guid id)
        {
            return localService.ValidarId(id);
        }

        public bool ValidarNomePost(PostLocalDto postLocalDto)
        {
            return mapper.Map<bool>(localService.ValidarNome(mapper.Map<Local>(postLocalDto)));
        }

        public bool ValidarNomePut(PutLocalDto putLocalDto)
        {
            return mapper.Map<bool>(localService.ValidarNome(mapper.Map<Local>(putLocalDto)));
        }

        public bool ExisteLocalId(Guid localId)
        {
            return localService.ExisteLocalId(localId);
        }
    }
}