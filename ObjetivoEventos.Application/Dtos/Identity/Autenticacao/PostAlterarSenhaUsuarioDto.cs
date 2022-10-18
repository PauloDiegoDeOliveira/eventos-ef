using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace ObjetivoEventos.Application.Dtos.Identity.Autenticacao
{
    /// <summary>
    /// Objeto utilizado para atualização de senha.
    /// </summary>
    public class PostAlterarSenhaUsuarioDto
    {
        [JsonIgnore]
        public Guid UsuarioId { get; set; }

        /// <summary>
        /// Senha Antiga
        /// </summary>
        [Display(Name = "Senha Antiga.")]
        [Required(ErrorMessage = "O campo senha antiga é obrigatório.")]
        public string SenhaAntiga { get; set; }

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
        [Required(ErrorMessage = "O campo confirmãção de senha é obrigatório.")]
        public string ConfirmarSenha { get; set; }
    }
}