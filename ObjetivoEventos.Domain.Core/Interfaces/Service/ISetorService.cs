using ObjetivoEventos.Domain.Core.Interfaces.Service.Base;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Pagination;
using ObjetivoEventos.Domain.Parameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Core.Interfaces.Service
{
    public interface ISetorService : IServiceBase<Setor>
    {
        Task<PagedList<Setor>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave);

        Task<List<Setor>> GetSetorByReservas(List<Guid> reservasId);

        Task<Setor> PostSetorByCadeirasAutomaticoAsync(Setor setor, List<GridParameters> gridParameters);

        Task<Setor> PutSetorByCadeirasAutomaticoAsync(Setor setor, List<GridParameters> gridParameters);

        bool ValidarId(Guid id);
    }
}