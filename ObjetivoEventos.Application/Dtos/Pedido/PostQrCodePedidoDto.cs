using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Pedido
{
    /// <summary>
    /// Objeto utilizado para inserção dos dados para gerar QRCode.
    /// </summary>
    public class PostQrCodePedidoDto
    {
        /// <summary>
        /// Id do Pedido
        /// </summary>
        /// <example>876777D9-EE18-4798-D3AE-08DA85F40146</example>
        [Display(Name = "Id do Pedido.")]
        [Required(ErrorMessage = "O campo id do pedido é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id do pedido está em um formato inválido.")]
        public Guid Id { get; set; }
    }
}