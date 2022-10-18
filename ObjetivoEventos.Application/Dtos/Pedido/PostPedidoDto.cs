using ObjetivoEventos.Application.Dtos.Reserva;
using ObjetivoEventos.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Pedido
{
    /// <summary>
    /// Objeto utilizado para inserção.
    /// </summary>
    public class PostPedidoDto
    {
        /// <summary>
        /// Reservas do pedido
        /// </summary>
        [Display(Name = "Reservas do pedido.")]
        [Required(ErrorMessage = "O campo reservas do pedido é obrigatório.")]
        public List<ReferenciaReservaDto> Reservas { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [Display(Name = "Status do pedido.")]
        [Required(ErrorMessage = "O campo status é obrigatório.")]
        [EnumDataType(typeof(Status), ErrorMessage = "O status é inválido.")]
        public Status Status { get; set; }
    }
}