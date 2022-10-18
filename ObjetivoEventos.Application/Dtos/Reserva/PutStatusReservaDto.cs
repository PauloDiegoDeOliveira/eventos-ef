using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Dtos.Reserva
{
    /// <summary>
    /// Objeto utilizado para alteração de status.
    /// </summary>
    public class PutStatusReservaDto : PutStatusDto
    {
        public PutStatusReservaDto()
        { }

        public PutStatusReservaDto(Guid id, Status status) : base(id, status)
        {
        }
    }
}