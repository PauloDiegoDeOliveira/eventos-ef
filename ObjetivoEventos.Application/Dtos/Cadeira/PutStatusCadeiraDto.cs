using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Dtos.Cadeira
{
    /// <summary>
    /// Objeto utilizado para alteração de status.
    /// </summary>
    public class PutStatusCadeiraDto : PutStatusDto
    {
        public PutStatusCadeiraDto()
        { }

        public PutStatusCadeiraDto(Guid id, Status status) : base(id, status)
        {
        }
    }
}