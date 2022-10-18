using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Pedido
{
    /// <summary>
    /// Objeto utilizado para atualização dos pedidos pelo QRCode.
    /// </summary>
    public class PutValidaQrCodePedidoDto
    {
        /// <summary>
        /// Id do Usuário
        /// </summary>
        /// <example>876777D9-EE18-4798-D3AE-08DA85F40146</example>
        [Display(Name = "Id do Usuario.")]
        [Required(ErrorMessage = "O campo id do usuário é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id do usuário está em um formato inválido.")]
        public Guid UsuarioId { get; set; }

        /// <summary>
        /// Id do Pedido
        /// </summary>
        /// <example>876777D9-EE18-4798-D3AE-08DA85F40146</example>
        [Display(Name = "Id do Pedido.")]
        [Required(ErrorMessage = "O campo id do pedido é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id do pedido está em um formato inválido.")]
        public Guid PedidoId { get; set; }

        /// <summary>
        /// Id da Reserva
        /// </summary>
        /// <example>876777D9-EE18-4798-D3AE-08DA85F40146</example>
        [Display(Name = "Id da Reserva.")]
        [Required(ErrorMessage = "O campo id da reserva é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id da reserva está em um formato inválido.")]
        public Guid ReservaId { get; set; }
    }
}