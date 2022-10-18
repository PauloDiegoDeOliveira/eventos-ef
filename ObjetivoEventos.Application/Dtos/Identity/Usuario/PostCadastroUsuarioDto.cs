using ObjetivoEventos.Application.Dtos.Endereco;
using ObjetivoEventos.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Identity.Usuario
{
    /// <summary>
    /// Objeto utilizado para inserção.
    /// </summary>
    public class PostCadastroUsuarioDto
    {
        /// <summary>
        /// Nome
        /// </summary>
        /// <example>Front</example>
        [Display(Name = "Nome do usuário.")]
        [Required(ErrorMessage = "O campo nome do usuário é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo nome do usuário precisa ter entre 3 ou 100 caracteres")]
        public string Nome { get; set; }

        /// <summary>
        /// Sobrenome
        /// </summary>
        /// <example>End</example>
        [Display(Name = "Sobrenome do usuário.")]
        [Required(ErrorMessage = "O campo sobrenome do usuário é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo sobrenome do usuário precisa ter entre 3 ou 100 caracteres")]
        public string Sobrenome { get; set; }

        /// <summary>
        /// RA
        /// </summary>
        /// <example>102030</example>
        [Display(Name = "RA do usuário.")]
        [Required(ErrorMessage = "O campo RA do usuário é obrigatório.")]
        public string RA { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        /// <example>edd.dev@unip.br</example>
        [Display(Name = "E-mail do usuário.")]
        [Required(ErrorMessage = "O campo e-mail é obrigatorio.")]
        [EmailAddress(ErrorMessage = "O campo e-mail está no formato inválido.")]
        public string Email { get; set; }

        /// <summary>
        /// Senha
        /// </summary>
        [Display(Name = "Senha do usuário.")]
        [Required(ErrorMessage = "O campo senha é obrigatorio.")]
        [StringLength(100, ErrorMessage = "O campo senha precisa ter entre {2} e {1} caracteres.", MinimumLength = 6)]
        public string Senha { get; set; }

        /// <summary>
        /// Confirmar senha
        /// </summary>
        [Display(Name = "Confirmar senha do usuário.")]
        [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
        [Required(ErrorMessage = "O campo confirmar senha é obrigatorio.")]
        public string ConfirmarSenha { get; set; }

        /// <summary>
        /// Apelido
        /// </summary>
        /// <example>Front-end</example>
        [Display(Name = "Apelido do usuário.")]
        public string Apelido { get; set; }

        /// <summary>
        /// CPF
        /// </summary>
        /// <example>55663601813</example>
        [Display(Name = "CPF do usuário.")]
        [Required(ErrorMessage = "O campo CPF do usuário é obrigatório.")]
        [RegularExpression(@"[0-9]{3}\.?[0-9]{3}\.?[0-9]{3}\-?[0-9]{2}", ErrorMessage = "O CPF do usuário está em formato inválido.")]
        public string CPF { get; set; }

        /// <summary>
        /// Data de nascimento
        /// </summary>
        /// <example>1984-05-20</example>
        [Display(Name = "Data de nascimento do usuário.")]
        [Required(ErrorMessage = "O campo data de nascimento do usuário é obrigatório.")]
        [ValidaDataInferior(ErrorMessage = "Certifique-se de que a data de nascimento é menor ou igual do que a data atual.")]
        public DateTime Nascimento { get; set; }

        /// <summary>
        /// Gênero
        /// </summary>
        /// <example>Masculino</example>
        [Display(Name = "Gênero do usuário.")]
        [Required(ErrorMessage = "O campo gênero do usuário é obrigatório.")]
        [EnumDataType(typeof(Genero), ErrorMessage = "O campo gênero é inválido.")]
        public Genero Genero { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <example>Ativo</example>
        [Display(Name = "Status do usuário.")]
        [Required(ErrorMessage = "O campo status do usuário é obrigatório.")]
        [EnumDataType(typeof(Status), ErrorMessage = "O campo status é inválido.")]
        public Status Status { get; set; }

        /// <summary>
        /// Endereço
        /// </summary>
        [Display(Name = "Endereço do usuário.")]
        public PostEnderecoDto Endereco { get; set; }
    }
}