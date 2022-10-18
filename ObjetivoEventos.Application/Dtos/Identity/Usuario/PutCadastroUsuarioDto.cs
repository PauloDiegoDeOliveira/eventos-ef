using ObjetivoEventos.Application.Dtos.Endereco;
using ObjetivoEventos.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Identity.Usuario
{
    /// <summary>
    /// Objeto utilizado para atualização.
    /// </summary>
    public class PutCadastroUsuarioDto
    {
        /// <summary>
        /// Id do Usuário
        /// </summary>
        /// <example>876777D9-EE18-4798-D3AE-08DA85F40146</example>
        [Display(Name = "Id do usuário.")]
        [Required(ErrorMessage = "O campo id do usuário é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id do usuário está em um formato inválido.")]
        public Guid Id { get; set; }

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
        /// Apelido
        /// </summary>
        /// <example>Front-end</example>
        [Display(Name = "Apelido do usuário.")]
        public string Apelido { get; private set; }

        /// <summary>
        /// CPF
        /// </summary>
        /// <example>556.636.018-13</example>
        [Display(Name = "CPF do usuário.")]
        [Required(ErrorMessage = "O campo CPF do usuário é obrigatório.")]
        [RegularExpression(@"[0-9]{3}\.?[0-9]{3}\.?[0-9]{3}\-?[0-9]{2}", ErrorMessage = "O CPF do usuário está em formato inválido.")]
        public string CPF { get; private set; }

        /// <summary>
        /// Data de nascimento
        /// </summary>
        /// <example>20-05-1984</example>
        [Display(Name = "Data de nascimento do usuário.")]
        [Required(ErrorMessage = "O campo data de nascimento do usuário é obrigatório.")]
        [ValidaDataInferior(ErrorMessage = "Certifique-se de que a data de nascimento é menor ou igual do que a data atual.")]
        public DateTime Nascimento { get; private set; }

        /// <summary>
        /// Gênero
        /// </summary>
        /// <example>Masculino</example>
        [Display(Name = "Gênero do usuário.")]
        [Required(ErrorMessage = "O campo gênero do usuário é obrigatório.")]
        [EnumDataType(typeof(Genero), ErrorMessage = "O campo gênero é inválido.")]
        public Genero Genero { get; private set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <example>Ativo</example>
        [Display(Name = "Status do usuário.")]
        [Required(ErrorMessage = "O campos tatus do usuário é obrigatório.")]
        [EnumDataType(typeof(Status), ErrorMessage = "O campo status é inválido.")]
        public Status Status { get; set; }

        /// <summary>
        /// Endereço
        /// </summary>
        [Display(Name = "Endereço do usuário.")]
        public PostEnderecoDto Endereco { get; set; }
    }
}