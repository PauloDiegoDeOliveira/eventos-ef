using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Dtos.Evento
{
    /// <summary>
    /// Objeto utilizado para alteração de status.
    /// </summary>
    public class PutStatusEventoDto : PutStatusDto
    {
        public PutStatusEventoDto()
        { }

        public PutStatusEventoDto(Guid id, Status status) : base(id, status)
        {
        }
    }
}