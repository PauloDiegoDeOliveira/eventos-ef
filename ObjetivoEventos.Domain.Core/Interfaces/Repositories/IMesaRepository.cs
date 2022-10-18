using ObjetivoEventos.Domain.Core.Interfaces.Repositories.Base;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Core.Interfaces.Repositories
{
    public interface IMesaRepository : IRepositoryBase<Mesa>
    {
        Task<PagedList<Mesa>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave);

        bool ValidaCadeiraRegistrada(Mesa mesa);

        bool ValidarId(Guid id);

        bool ValidarFileiraEColuna(Mesa mesa);
    }
}