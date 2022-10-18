using ObjetivoEventos.Application.Dtos.Reserva;
using ObjetivoEventos.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ObjetivoEventos.Application.Dtos.Pedido
{
    /// <summary>
    /// Objeto utilizado para visualização.
    /// </summary>
    public class ViewPedidoDto
    {
        public Guid Id { get; set; }

        public Guid UsuarioId { get; set; }

        public float ValorTotal { get; set; }

        public long Numero { get; set; }

        public SituacaoPedido SituacaoPedido { get; set; }

        public Status Status { get; set; }

        public List<ReferenciaReservaDto> Reservas { get; set; }
    }
}