using ObjetivoEventos.Domain.Core.Interfaces.Repositories.Base;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Core.Interfaces.Repositories
{
    public interface IPedidoRepository : IRepositoryBase<Pedido>
    {
        Task<PagedList<Pedido>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave);

        Task<List<Pedido>> GetByUsuarioAutenticadoAsync();

        Task<Pedido> GetDetalhesByIdAsync(Guid id);

        Task<List<Pedido>> GetPedidosAtivosSituacaoPedidoAsync(SituacaoPedido situacaoPedido);

        Task<List<Pedido>> GetPedidosVencidosPorDiasAsync(int dias);

        Task<List<Pedido>> VerificaPedidosVencidosPeloEventoAsync(bool pedidosVencidos, List<Pedido> pedidos);

        Task<Pedido> PutTrocaPedidoReserva(Guid pedidoId, Guid reservaAntigaId, Guid novaReservaId);

        Task<Pedido> PutSituacaoPedidoAsync(Pedido pedido);

        Task<List<Pedido>> PutPedidosAtivosParaInativosByEventosExpirados(List<Evento> eventos);

        bool ValidarId(Guid id);

        bool ValidarInputReservaEventoPedidos(Pedido pedido);

        bool ValidarReservaPedidos(Pedido pedido);
    }
}