using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Identity.Autenticacao
{
    /// <summary>
    /// Objeto utilizado para inserção de e-mail.
    /// </summary>
    public class PostEmailDto
    {
        /// <summary>
        /// E-mail
        /// </summary>
        /// <example>edd.dev@unip.br</example>
        [Display(Name = "E-mail.")]
        [Required(ErrorMessage = "O campo e-mail é obrigatorio.")]
        [EmailAddress(ErrorMessage = "O campo e-mail está em um formato inválido.")]
        public string Email { get; set; }
    }
}