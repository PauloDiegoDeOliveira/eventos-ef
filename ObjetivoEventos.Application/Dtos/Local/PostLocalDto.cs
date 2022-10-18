using ObjetivoEventos.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Local
{
    /// <summary>
    /// Objeto utilizado para inserção.
    /// </summary>
    public class PostLocalDto
    {
        /// <summary>
        /// Nome
        /// </summary>
        /// <example>Cidade Universitária</example>
        [Display(Name = "Nome do local.")]
        [Required(ErrorMessage = "O campo nome do local é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo nome do local precisa ter entre 3 ou 100 caracteres")]
        public string Nome { get; set; }

        /// <summary>
        /// Telefone
        /// </summary>
        /// <example>(11) 3767-5800</example>
        [Display(Name = "Telefone do local.")]
        [RegularExpression(@"^\([1-9]{2}\) (?:[2-9]|9[1-9])[0-9]{3}\-[0-9]{4}$", ErrorMessage = "O campo telefone do local está em formato incorreto.")]
        public string Telefone { get; set; }

        /// <summary>
        /// CEP
        /// </summary>
        /// <example>05347-020</example>
        [Display(Name = "CEP do local.")]
        [Required(ErrorMessage = "O campo CEP do local é obrigatório.")]
        [RegularExpression(@"^\d{5}-\d{3}$", ErrorMessage = "O campo CEP do local em formato incorreto.")]
        public string CEP { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <example>Ativo</example>
        [Display(Name = "Status do local.")]
        [Required(ErrorMessage = "O campo status do local é obrigatório.")]
        [EnumDataType(typeof(Status), ErrorMessage = "O campo status é inválido.")]
        public Status Status { get; set; }
    }
}