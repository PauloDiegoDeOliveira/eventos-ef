using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Mesa;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Interfaces.Base;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Interfaces
{
    public interface IMesaApplication : IApplicationBase<Mesa, ViewMesaDto, PostMesaDto, PutMesaDto, PutStatusDto>
    {
        Task<ViewPagedListDto<Mesa, ViewMesaDto>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave);

        bool ValidarId(Guid id);

        bool ValidaCadeiraRegistrada(PutMesaDto putMesaDto);

        bool ValidarFileiraEColunaPost(PostMesaDto postMesaDto);

        bool ValidarFileiraEColunaPut(PutMesaDto putMesaDto);
    }
}