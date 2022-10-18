using Microsoft.Extensions.DependencyInjection;
using ObjetivoEventos.Application.Dtos.Evento;
using ObjetivoEventos.Application.Dtos.Identity.Usuario;
using ObjetivoEventos.Application.Dtos.Pedido;
using ObjetivoEventos.Application.Interfaces;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.BackgroundServices
{
    public class PedidoBackgroundService
    {
        private readonly IServiceScope scope;
        private readonly IPedidoApplication pedidoApplication;
        private readonly IUsuarioApplication usuarioApplication;

        public PedidoBackgroundService(IServiceProvider serviceProvider)
        {
            scope = serviceProvider.CreateScope();
            pedidoApplication = scope.ServiceProvider.GetRequiredService<IPedidoApplication>();
            usuarioApplication = scope.ServiceProvider.GetRequiredService<IUsuarioApplication>();
        }

        public async Task<List<ViewPedidoDto>> PutPedidosAtivosParaInativosByEventosExpirados(List<ViewEventoDto> eventos)
        {
            return await pedidoApplication.PutPedidosAtivosParaInativosByEventosExpirados(eventos);
        }

        public async Task CancelaPedidosNaoPagosPorDias()
        {
            List<ViewPedidoDto> pedidos = await pedidoApplication.GetPedidosVencidosPorDiasAsync(4);

            if (pedidos != null)
            {
                foreach (ViewPedidoDto pedido in pedidos)
                {
                    await pedidoApplication.PutSituacaoPedidoAsync(new PutSituacaoPedidoDto(pedido.Id, SituacaoPedido.Cancelado));
                }
            }
        }

        public async Task NotificaPedidosNaoPagos()
        {
            List<Pedido> pedidos = await pedidoApplication.GetPedidosAtivosSituacaoPedidoAsync(SituacaoPedido.AguardandoPagamento);

            if (pedidos != null)
            {
                List<ViewPedidoDto> viewPedidoDtos = await pedidoApplication.VerificaPedidosVencidosPeloEventoAsync(true, pedidos);

                if (viewPedidoDtos != null && viewPedidoDtos.Count > 0)
                {
                    foreach (var viewPedidoDto in viewPedidoDtos)
                    {
                        ViewUsuarioDto user = await usuarioApplication.GetByIdAsync(viewPedidoDto.UsuarioId);

                        if (user != null)
                            await pedidoApplication.CriarEmail("Olá {{UserName}}, você ainda não realizou o pagamento do seu pedido (Objetivo Eventos).", "WaitingPayment", user.Nome, viewPedidoDto.Numero.ToString(), new List<string> { user.Email });
                    }
                }

                viewPedidoDtos.Clear();
            }
        }
    }
}