using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Identity.Autenticacao
{
    /// <summary>
    /// Objeto utilizado para confirmação de e-mail.
    /// </summary>
    public class PostConfirmacaoEmailDto
    {
        /// <summary>
        /// Id do Usuário
        /// </summary>
        /// <example>876777D9-EE18-4798-D3AE-08DA85F40146</example>
        [Display(Name = "Usuário Id.")]
        [Required(ErrorMessage = "O campo id do usuário é obrigatorio.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id do usuário está em um formato inválido.")]
        public Guid UsuarioId { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        [Display(Name = "Token.")]
        [Required(ErrorMessage = "O campo token é obrigatorio.")]
        public string Token { get; set; }
    }
}