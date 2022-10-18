using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Pedido
{
    /// <summary>
    /// Objeto utilizado para atualização de troca.
    /// </summary>
    public class PutPedidoTrocaDto
    {
        /// <summary>
        /// Id do Pedido
        /// </summary>
        /// <example>876777D9-EE18-4798-D3AE-08DA85F40146</example>
        [Display(Name = "Id do Pedido.")]
        [Required(ErrorMessage = "O campo id do pedido é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id do pedido está em um formato inválido.")]
        public Guid PedidoId { get; set; }

        /// <summary>
        /// Reserva a ser removida
        /// </summary>
        /// <example>876777D9-EE18-4798-D3AE-08DA85F40146</example>
        [Display(Name = "Reserva Antiga.")]
        [Required(ErrorMessage = "O campo reserva antiga do pedido é obrigatório.")]
        public Guid ReservaAntiga { get; set; }

        /// <summary>
        /// Nova reserva
        /// </summary>
        /// <example>876777D9-EE18-4798-D3AE-08DA85F40146</example>
        [Display(Name = "Nova Reserva.")]
        [Required(ErrorMessage = "O campo nova reserva do pedido é obrigatório.")]
        public Guid NovaReserva { get; set; }
    }
}