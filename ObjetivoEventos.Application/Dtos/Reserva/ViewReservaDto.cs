using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Dtos.Reserva
{
    /// <summary>
    /// Objeto utilizado para visualização.
    /// </summary>
    public class ViewReservaDto
    {
        public Guid Id { get; set; }

        public Guid EventoId { get; set; }

        public Guid LocalId { get; set; }

        public Guid SetorId { get; set; }

        public Guid? MesaId { get; set; }

        public Guid CadeiraId { get; set; }

        public Guid UsuarioId { get; set; }

        public string ConnectionId { get; set; }

        public SituacaoReserva SituacaoReserva { get; set; }

        public Status Status { get; set; }
    }
}