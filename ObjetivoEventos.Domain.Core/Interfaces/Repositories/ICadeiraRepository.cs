using ObjetivoEventos.Domain.Core.Interfaces.Repositories.Base;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Core.Interfaces.Repositories
{
    public interface ICadeiraRepository : IRepositoryBase<Cadeira>
    {
        Task<PagedList<Cadeira>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave);

        Task<List<Cadeira>> GetByColunaLinhaAsync(List<Cadeira> cadeiras);

        Task<List<Cadeira>> PostAutomaticoAsync(List<Cadeira> cadeiras);

        bool ValidarId(Guid id);

        bool ValidarFileiraEColuna(Cadeira cadeira);
    }
}