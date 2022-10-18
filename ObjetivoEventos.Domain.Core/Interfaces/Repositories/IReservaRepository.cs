using ObjetivoEventos.Domain.Core.Interfaces.Repositories.Base;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Core.Interfaces.Repositories
{
    public interface IReservaRepository : IRepositoryBase<Reserva>
    {
        Task<PagedList<Reserva>> GetPaginationAsync(ParametersBase parametersBase);

        Task<Reserva> GetReservasDetalhesById(Guid id);

        Task<List<Reserva>> GetReservasByTempoSituacaoAsync(int minutos, SituacaoReserva situacaoReserva);

        Task<List<Reserva>> GetReservasByListId(List<Guid> ids);

        Task<List<Reserva>> PutStatusRangeAsync(List<Guid> ids, Status status);

        Task<List<Reserva>> PutRangeAsync(List<Reserva> reservas);

        Task<List<Reserva>> DeleteRangeAsync(List<Reserva> reservas);

        Task<List<Reserva>> DeleteByConnectionId(string connectionId);

        bool VerificaDisponibilidadeReserva(Reserva reserva);

        bool ValidarSetorCadeiraMesa(Reserva reserva);

        bool ValidaListId(List<Guid> ids);

        bool ValidaQuantidadeCadeiras(Reserva reserva);

        bool ValidarId(Guid id);
    }
}