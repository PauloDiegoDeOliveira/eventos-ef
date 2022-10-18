using ObjetivoEventos.Domain.Core.Interfaces.Repositories.Base;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Core.Interfaces.Repositories
{
    public interface ILocalRepository : IRepositoryBase<Local>
    {
        Task<PagedList<Local>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave);

        bool ValidarId(Guid id);

        bool ValidarNome(Local local);

        bool ExisteLocalId(Guid localId);
    }
}