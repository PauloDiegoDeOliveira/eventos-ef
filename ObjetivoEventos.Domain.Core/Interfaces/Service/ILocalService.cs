using ObjetivoEventos.Domain.Core.Interfaces.Service.Base;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Core.Interfaces.Service
{
    public interface ILocalService : IServiceBase<Local>
    {
        Task<PagedList<Local>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave);

        bool ValidarId(Guid id);

        bool ValidarNome(Local local);

        bool ExisteLocalId(Guid localId);
    }
}