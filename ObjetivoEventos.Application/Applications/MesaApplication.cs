using AutoMapper;
using ObjetivoEventos.Application.Applications.Base;
using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Mesa;
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
    public class MesaApplication : ApplicationBase<Mesa, ViewMesaDto, PostMesaDto, PutMesaDto, PutStatusDto>, IMesaApplication
    {
        private readonly IMesaService serviceMesa;

        public MesaApplication(IMesaService serviceMesa,
                               INotificador notificador,
                               IMapper mapper) : base(serviceMesa, notificador, mapper)
        {
            this.serviceMesa = serviceMesa;
        }

        public async Task<ViewPagedListDto<Mesa, ViewMesaDto>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            PagedList<Mesa> pagedList = await serviceMesa.GetPaginationAsync(parametersPalavraChave);
            return new ViewPagedListDto<Mesa, ViewMesaDto>(pagedList, mapper.Map<List<ViewMesaDto>>(pagedList));
        }

        public bool ValidarId(Guid id)
        {
            return serviceMesa.ValidarId(id);
        }

        public bool ValidaCadeiraRegistrada(PutMesaDto putMesaDto)
        {
            return mapper.Map<bool>(serviceMesa.ValidaCadeiraRegistrada(mapper.Map<Mesa>(putMesaDto)));
        }

        public bool ValidarFileiraEColunaPost(PostMesaDto postMesaDto)
        {
            return mapper.Map<bool>(serviceMesa.ValidarFileiraEColuna(mapper.Map<Mesa>(postMesaDto)));
        }

        public bool ValidarFileiraEColunaPut(PutMesaDto putMesaDto)
        {
            return mapper.Map<bool>(serviceMesa.ValidarFileiraEColuna(mapper.Map<Mesa>(putMesaDto)));
        }
    }
}