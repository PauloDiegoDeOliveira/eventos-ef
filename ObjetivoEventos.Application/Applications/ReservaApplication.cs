using AutoMapper;
using ObjetivoEventos.Application.Applications.Base;
using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Dtos.Reserva;
using ObjetivoEventos.Application.Hubs;
using ObjetivoEventos.Application.Interfaces;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Applications
{
    public class ReservaApplication : ApplicationBase<Reserva, ViewReservaDto, PostReservaDto, PutReservaDto, PutStatusDto>, IReservaApplication
    {
        private readonly IReservaService reservaService;
        private readonly MessageHub messageHub;

        public ReservaApplication(IReservaService reservaService,
                                  MessageHub messageHub,
                                  INotificador notificador,
                                  IMapper mapper) : base(reservaService, notificador, mapper)

        {
            this.messageHub = messageHub;
            this.reservaService = reservaService;
        }

        public async Task<ViewPagedListDto<Reserva, ViewReservaDto>> GetPaginationAsync(ParametersBase parametersBase)
        {
            PagedList<Reserva> pagedList = await reservaService.GetPaginationAsync(parametersBase);
            return new ViewPagedListDto<Reserva, ViewReservaDto>(pagedList, mapper.Map<List<ViewReservaDto>>(pagedList));
        }

        public async Task<Reserva> GetReservasDetalhesById(Guid id)
        {
            return await reservaService.GetReservasDetalhesById(id);
        }

        public async Task<List<ViewReservaDto>> GetReservasByTempoSituacaoAsync(int minutos, SituacaoReserva situacaoReserva)
        {
            return mapper.Map<List<ViewReservaDto>>(await reservaService.GetReservaByTempoSituacaoAsync(minutos, situacaoReserva));
        }

        public async Task<List<ViewReservaDto>> PutStatusRangeAsync(List<Guid> ids, Status status)
        {
            return mapper.Map<List<ViewReservaDto>>(await reservaService.PutStatusRangeAsync(ids, status));
        }

        public async Task<List<ViewReservaDto>> PutSituacaoReservaAsync(PutSituacaoReservaDto putSituacaoReservaDto)
        {
            List<ViewReservaDto> reservas = mapper.Map<List<ViewReservaDto>>(await reservaService.PutSituacaoReservaAsync(putSituacaoReservaDto.Ids, putSituacaoReservaDto.SituacaoReserva));

            if (reservas == null)
                return null;

            foreach (ViewReservaDto reserva in reservas)
            {
                if (reserva.SituacaoReserva == SituacaoReserva.Cancelada)
                    await messageHub.SendMessage(reserva);
            }

            return reservas;
        }

        public override async Task<ViewReservaDto> DeleteAsync(Guid id)
        {
            Reserva reserva = await reservaService.GetReservasDetalhesById(id);

            if (reserva.Pedidos.Count > 0)
            {
                Notificar("Não é possivel deletar esta reserva porque ela já está vinculada a um pedido.");
                return null;
            }

            ViewReservaDto reservaExcluida = await base.DeleteAsync(id);

            if (reservaExcluida == null)
            {
                Notificar("Nenhuma reserva foi encontrada com o id informado.");
                return null;
            }

            return reservaExcluida;
        }

        public async Task<List<ViewReservaDto>> DeleteRangeAsync(List<ViewReservaDto> viewReservaDtos)
        {
            return mapper.Map<List<ViewReservaDto>>(await reservaService.DeleteRangeAsync(mapper.Map<List<Reserva>>(viewReservaDtos)));
        }

        public async Task<List<ViewReservaDto>> DeleteByConnectionId(string connectionId)
        {
            return mapper.Map<List<ViewReservaDto>>(await reservaService.DeleteByConnectionId(connectionId));
        }

        public bool VerificaDisponibilidadeReserva(PostReservaDto postReservaDto)
        {
            return reservaService.VerificaDisponibilidadeReserva(mapper.Map<Reserva>(postReservaDto));
        }

        public bool ValidarSetorCadeiraMesa(PostReservaDto postReservaDto)
        {
            return reservaService.ValidarSetorCadeiraMesa(mapper.Map<Reserva>(postReservaDto));
        }

        public bool ValidaListId(List<Guid> ids)
        {
            return reservaService.ValidaListId(ids);
        }

        public bool ValidaQuantidadeCadeiras(PostReservaDto postReservaDto)
        {
            return reservaService.ValidaQuantidadeCadeiras(mapper.Map<Reserva>(postReservaDto));
        }

        public bool ValidarId(Guid id)
        {
            return reservaService.ValidarId(id);
        }
    }
}