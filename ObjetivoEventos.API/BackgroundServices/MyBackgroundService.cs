using Microsoft.Extensions.Hosting;
using ObjetivoEventos.Application.Dtos.Evento;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.BackgroundServices
{
    public class MyBackgroundService : BackgroundService
    {
        private readonly ReservaBackgroundService reservaBackgroundService;
        private readonly PedidoBackgroundService pedidoBackgroundService;
        private readonly EventoBackgroundService eventoBackgroundService;
        private static DateTime nextDay;

        public MyBackgroundService(ReservaBackgroundService reservaBackgroundService,
            PedidoBackgroundService pedidoBackgroundService,
            EventoBackgroundService eventoBackgroundService)
        {
            this.reservaBackgroundService = reservaBackgroundService;
            this.pedidoBackgroundService = pedidoBackgroundService;
            this.eventoBackgroundService = eventoBackgroundService;
            nextDay = DateTime.Today;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await reservaBackgroundService.VerificaReservasExpiradas();
                await pedidoBackgroundService.CancelaPedidosNaoPagosPorDias();

                if (nextDay == DateTime.Today)
                {
                    List<ViewEventoDto> eventos = await eventoBackgroundService.GetEventosExpiradosAsync();

                    if (eventos != null && eventos.Count > 0)
                        await pedidoBackgroundService.PutPedidosAtivosParaInativosByEventosExpirados(eventos);

                    await pedidoBackgroundService.NotificaPedidosNaoPagos();
                    await eventoBackgroundService.InativaEventosExpiradosAsync(eventos);

                    nextDay = DateTime.Today.AddDays(1);
                }

                await Task.Delay(TimeSpan.FromSeconds(60), stoppingToken);
            }
        }
    }
}