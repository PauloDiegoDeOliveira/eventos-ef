using ObjetivoEventos.Domain.Core.Interfaces.Service.Base;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Core.Interfaces.Service
{
    public interface ICadeiraService : IServiceBase<Cadeira>
    {
        Task<PagedList<Cadeira>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave);

        Task<List<Cadeira>> PostAutomaticoAsync(List<Cadeira> cadeiras);

        bool ValidarId(Guid id);

        bool ValidarFileiraEColuna(Cadeira cadeira);
    }
}