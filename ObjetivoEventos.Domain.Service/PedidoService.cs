using ObjetivoEventos.Domain.Core.Interfaces.Repositories;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using ObjetivoEventos.Domain.Service.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Service
{
    public class PedidoService : ServiceBase<Pedido>, IPedidoService
    {
        private readonly IPedidoRepository pedidoRepository;

        public PedidoService(IPedidoRepository pedidoRepository, INotificador notificador) : base(pedidoRepository, notificador)
        {
            this.pedidoRepository = pedidoRepository;
        }

        public async Task<PagedList<Pedido>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            return await pedidoRepository.GetPaginationAsync(parametersPalavraChave);
        }

        public async Task<List<Pedido>> GetByUsuarioAutenticadoAsync()
        {
            return await pedidoRepository.GetByUsuarioAutenticadoAsync();
        }

        public async Task<List<Pedido>> GetPedidosAtivosSituacaoPedidoAsync(SituacaoPedido situacaoPedido)
        {
            return await pedidoRepository.GetPedidosAtivosSituacaoPedidoAsync(situacaoPedido);
        }

        public async Task<List<Pedido>> GetPedidosVencidosPorDiasAsync(int dias)
        {
            return await pedidoRepository.GetPedidosVencidosPorDiasAsync(dias);
        }

        public async Task<List<Pedido>> VerificaPedidosVencidosPeloEventoAsync(bool pedidosVencidos, List<Pedido> pedidos)
        {
            return await pedidoRepository.VerificaPedidosVencidosPeloEventoAsync(pedidosVencidos, pedidos);
        }

        public async Task<Pedido> GetDetalhesByIdAsync(Guid id)
        {
            return await pedidoRepository.GetDetalhesByIdAsync(id);
        }

        public async Task<Pedido> PutTrocaPedidoReserva(Guid pedidoId, Guid reservaAntigaId, Guid novaReservaId)
        {
            return await pedidoRepository.PutTrocaPedidoReserva(pedidoId, reservaAntigaId, novaReservaId);
        }

        public async Task<Pedido> PutSituacaoPedidoAsync(Guid id, SituacaoPedido situacaoPedido)
        {
            Pedido pedido = await pedidoRepository.GetDetalhesByIdAsync(id);

            if (pedido is null)
            {
                Notificar("Nenhum pedido foi encontrado com o id informado.");
                return null;
            }

            if (pedido.VerificaRegraSituacaoPedido(situacaoPedido.ToString()))
            {
                pedido.AlterarSituacaoPedido(situacaoPedido.ToString());
            }
            else
            {
                Notificar($"Não é permitido atualizar o status da venda de {pedido.SituacaoPedido} para {situacaoPedido}");
                return null;
            }

            return await pedidoRepository.PutSituacaoPedidoAsync(pedido);
        }

        public async Task<List<Pedido>> PutPedidosAtivosParaInativosByEventosExpirados(List<Evento> eventos)
        {
            return await pedidoRepository.PutPedidosAtivosParaInativosByEventosExpirados(eventos);
        }

        public bool ValidarId(Guid id)
        {
            return pedidoRepository.ValidarId(id);
        }

        public bool ValidarInputReservaEventoPedidos(Pedido pedido)
        {
            return pedidoRepository.ValidarInputReservaEventoPedidos(pedido);
        }

        public bool ValidarReservaPedidos(Pedido pedido)
        {
            return pedidoRepository.ValidarReservaPedidos(pedido);
        }
    }
}