using ObjetivoEventos.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Endereco
{
    /// <summary>
    /// Objeto utilizado para inserção.
    /// </summary>
    public class PostEnderecoDto
    {
        /// <summary>
        /// CEP
        /// </summary>
        /// <example>05347-020</example>
        [Display(Name = "CEP do endereço.")]
        [Required(ErrorMessage = "O campo CEP do endereço é obrigatório.")]
        [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "O campo CEP do endereço em formato incorreto.")]
        public string CEP { get; set; }

        /// <summary>
        /// Estado
        /// </summary>
        /// <example>SP</example>
        [Display(Name = "Estado do endereço.")]
        [Required(ErrorMessage = "O campo estado do endereço é obrigatório.")]
        [EnumDataType(typeof(Estado), ErrorMessage = "O campo estado é inválido.")]
        public Estado Estado { get; set; }

        /// <summary>
        /// Cidade
        /// </summary>
        /// <example>São Paulo</example>
        [Display(Name = "Cidade do endereço.")]
        [Required(ErrorMessage = "O campo cidade do endereço é obrigatório.")]
        public string Cidade { get; set; }

        /// <summary>
        /// Logradouro
        /// </summary>
        /// <example>Av. Torres de Oliveira</example>
        [Display(Name = "Logradouro do endereço.")]
        [Required(ErrorMessage = "O campo logradouro do endereço é obrigatório.")]
        public string Logradouro { get; set; }

        /// <summary>
        /// Número
        /// </summary>
        /// <example>330</example>
        [Display(Name = "Número do endereço.")]
        [Required(ErrorMessage = "O campo número do endereço é obrigatório.")]
        public string Numero { get; set; }

        /// <summary>
        /// Complemento
        /// </summary>
        /// <example>Apto 1</example>
        [Display(Name = "Complemento do endereço.")]
        public string Complemento { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <example>Ativo</example>
        [Display(Name = "Status do endereço.")]
        [Required(ErrorMessage = "O campo status do endereço é obrigatório.")]
        [EnumDataType(typeof(Status), ErrorMessage = "O status é inválido.")]
        public Status Status { get; set; }
    }
}