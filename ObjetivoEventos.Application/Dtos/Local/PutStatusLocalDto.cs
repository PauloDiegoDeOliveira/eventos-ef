using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Dtos.Local
{
    /// <summary>
    /// Objeto utilizado para alteração de status.
    /// </summary>
    public class PutStatusLocalDto : PutStatusDto
    {
        public PutStatusLocalDto()
        { }

        public PutStatusLocalDto(Guid id, Status status) : base(id, status)
        {
        }
    }
}