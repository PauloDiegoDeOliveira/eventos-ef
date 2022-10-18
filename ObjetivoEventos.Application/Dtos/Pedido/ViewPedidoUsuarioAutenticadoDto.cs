using ObjetivoEventos.Application.Dtos.Evento;
using ObjetivoEventos.Application.Dtos.Reserva;
using ObjetivoEventos.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ObjetivoEventos.Application.Dtos.Pedido
{
    /// <summary>
    /// Objeto utilizado para visualização de detalhes do pedido autenticado.
    /// </summary>
    public class ViewPedidoUsuarioAutenticadoDto
    {
        public Guid Id { get; set; }

        public long Numero { get; set; }

        public float ValorTotal { get; set; }

        public SituacaoPedido SituacaoPedido { get; set; }

        public Status Status { get; set; }

        public ViewEventoUsuarioAutenticadoDto Evento { get; set; }

        public List<ViewReservaQrDto> Reservas { get; set; }

        public ViewPedidoUsuarioAutenticadoDto(Domain.Entitys.Pedido pedido, ViewEventoUsuarioAutenticadoDto evento, List<ViewReservaQrDto> reservaQrDtos)
        {
            Id = pedido.Id;
            Numero = pedido.Numero;
            _ = Enum.TryParse(pedido.SituacaoPedido, out SituacaoPedido situacao);
            this.SituacaoPedido = situacao;
            _ = Enum.TryParse(pedido.Status, out Status status);
            Status = status;
            Evento = evento;
            Reservas = reservaQrDtos;
        }
    }
}