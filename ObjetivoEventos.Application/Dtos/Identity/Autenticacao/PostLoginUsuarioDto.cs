using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Identity.Autenticacao
{
    /// <summary>
    /// Objeto utilizado para realização do login.
    /// </summary>
    public class PostLoginUsuarioDto
    {
        /// <summary>
        /// E-mail
        /// </summary>
        /// <example>edd.dev@unip.br</example>
        [Display(Name = "E-mail do usuário.")]
        [Required(ErrorMessage = "O campo e-mail é obrigatorio.")]
        [EmailAddress(ErrorMessage = "O campo e-mail está em um formato inválido.")]
        public string Email { get; set; }

        /// <summary>
        /// Senha
        /// </summary>
        [Display(Name = "Senha do usuário.")]
        [Required(ErrorMessage = "O campo senha é obrigatorio.")]
        public string Senha { get; set; }
    }
}