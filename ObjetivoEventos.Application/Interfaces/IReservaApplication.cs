using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Dtos.Reserva;
using ObjetivoEventos.Application.Interfaces.Base;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Interfaces
{
    public interface IReservaApplication : IApplicationBase<Reserva, ViewReservaDto, PostReservaDto, PutReservaDto, PutStatusDto>
    {
        Task<ViewPagedListDto<Reserva, ViewReservaDto>> GetPaginationAsync(ParametersBase parametersBase);

        Task<ViewValorTotalReservaDto> GetValorTotal(PostListaIdReservaDto postListaIdReservaDto);

        Task<List<ViewReservaDto>> GetReservasByTempoSituacaoAsync(int minutos, SituacaoReserva situacaoReserva);

        Task<Reserva> GetReservasDetalhesById(Guid id);

        Task<List<ViewReservaDto>> PutStatusRangeAsync(List<Guid> ids, Status status);

        Task<List<ViewReservaDto>> PutSituacaoReservaAsync(PutSituacaoReservaDto putSituacaoReservaDto);

        Task<List<ViewReservaDto>> DeleteRangeAsync(List<ViewReservaDto> viewReservaDtos);

        Task<List<ViewReservaDto>> DeleteByConnectionId(string connectionId);

        bool VerificaDisponibilidadeReserva(PostReservaDto postReservaDto);

        bool ValidarSetorCadeiraMesa(PostReservaDto postReservaDto);

        bool ValidaListId(List<Guid> ids);

        bool ValidaQuantidadeCadeiras(PostReservaDto postReservaDto);

        bool ValidarId(Guid id);
    }
}