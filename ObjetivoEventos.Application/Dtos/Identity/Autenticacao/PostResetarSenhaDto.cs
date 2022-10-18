using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Identity.Autenticacao
{
    /// <summary>
    /// Objeto utilizado para resetar a senha pelo e-mail.
    /// </summary>
    public class PostResetarSenhaDto
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

        /// <summary>
        /// Nova Senha
        /// </summary>
        [Display(Name = "Nova Senha.")]
        [Required(ErrorMessage = "O campo nova senha é obrigatorio.")]
        [StringLength(100, ErrorMessage = "O campo nova senha precisa ter entre {2} e {1} caracteres.", MinimumLength = 6)]
        public string NovaSenha { get; set; }

        /// <summary>
        /// Confirmar Senha
        /// </summary>
        [Display(Name = "Confirmar Senha.")]
        [Compare("NovaSenha", ErrorMessage = "As senhas não conferem.")]
        [Required(ErrorMessage = "O campo confirmar senha é obrigatorio.")]
        public string ConfirmarSenha { get; set; }
    }
}