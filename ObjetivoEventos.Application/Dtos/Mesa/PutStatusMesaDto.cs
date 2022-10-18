using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Dtos.Mesa
{
    /// <summary>
    /// Objeto utilizado para alteração de status.
    /// </summary>
    public class PutStatusMesaDto : PutStatusDto
    {
        public PutStatusMesaDto()
        {
        }

        public PutStatusMesaDto(Guid id, Status status) : base(id, status)
        {
        }
    }
}