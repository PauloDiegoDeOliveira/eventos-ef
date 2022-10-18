using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Local;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Interfaces.Base;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Interfaces
{
    public interface ILocalApplication : IApplicationBase<Local, ViewLocalDto, PostLocalDto, PutLocalDto, PutStatusDto>
    {
        Task<ViewPagedListDto<Local, ViewLocalDto>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave);

        bool ValidarId(Guid id);

        bool ValidarNomePost(PostLocalDto postLocalDto);

        bool ValidarNomePut(PutLocalDto putLocalDto);

        bool ExisteLocalId(Guid localId);
    }
}