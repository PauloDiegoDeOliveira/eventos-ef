using ObjetivoEventos.Domain.Core.Interfaces.Repositories.Base;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Core.Interfaces.Repositories
{
    public interface IEventoRepository : IRepositoryBase<Evento>
    {
        Task<PagedList<Evento>> GetPaginationAsync(ParametersEvento parametersEvento);

        Task<Evento> GetDisponibilidadeAsync(Guid eventoId, Guid setorId);

        Task<Evento> GetDetalhesAsync(Guid eventoId);

        Task<List<Evento>> GetEventosExpiradosAsync();

        Task<List<Evento>> PutStatusRangeAsync(List<Guid> ids, Status status);

        bool ValidarId(Guid id);

        bool ValidarDataHora(Evento evento);
    }
}