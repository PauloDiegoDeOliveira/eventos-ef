using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Dtos.Pedido
{
    /// <summary>
    /// Objeto utilizado para alteração de status.
    /// </summary>
    public class PutStatusPedidoDto : PutStatusDto
    {
        public PutStatusPedidoDto()
        { }

        public PutStatusPedidoDto(Guid id, Status status) : base(id, status)
        {
        }
    }
}