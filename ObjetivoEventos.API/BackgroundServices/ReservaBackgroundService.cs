using Microsoft.Extensions.DependencyInjection;
using ObjetivoEventos.Application.Dtos.Reserva;
using ObjetivoEventos.Application.Hubs;
using ObjetivoEventos.Application.Interfaces;
using ObjetivoEventos.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.BackgroundServices
{
    public class ReservaBackgroundService
    {
        private readonly IServiceScope serviceScope;
        private readonly IReservaApplication reservaApplication;
        private readonly MessageHub messageHub;

        public ReservaBackgroundService(IServiceProvider serviceProvider)
        {
            serviceScope = serviceProvider.CreateScope();
            reservaApplication = serviceScope.ServiceProvider.GetRequiredService<IReservaApplication>();
            messageHub = serviceScope.ServiceProvider.GetRequiredService<MessageHub>();
        }

        public async Task VerificaReservasExpiradas()
        {
            List<ViewReservaDto> viewReservadosDtos = await reservaApplication.GetReservasByTempoSituacaoAsync(5, SituacaoReserva.Reservado);

            if (viewReservadosDtos != null && viewReservadosDtos.Count > 0)
            {
                await DeleteRangeAsync(viewReservadosDtos);
                viewReservadosDtos.Clear();
            }
        }

        public async Task<List<ViewReservaDto>> DeleteRangeAsync(List<ViewReservaDto> viewReservaDtos)
        {
            List<ViewReservaDto> excluidos = await reservaApplication.DeleteRangeAsync(viewReservaDtos);

            foreach (ViewReservaDto viewReservaDto in excluidos)
            {
                await messageHub.SendMessage(viewReservaDto);
            }

            return excluidos;
        }
    }
}