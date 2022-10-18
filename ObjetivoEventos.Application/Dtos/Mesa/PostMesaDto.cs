using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Domain.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Mesa
{
    /// <summary>
    /// Objeto utilizado para inserção.
    /// </summary>
    public class PostMesaDto
    {
        /// <summary>
        /// Nome
        /// </summary>
        /// <example>Mesa</example>
        [Display(Name = "Nome da mesa.")]
        [Required(ErrorMessage = "O campo nome da mesa é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo nome da mesa precisa ter entre 3 ou 100 caracteres")]
        public string Nome { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        /// <example>Mesa 1</example>
        [Display(Name = "Alias da mesa.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo alias da mesa precisa ter entre 3 ou 100 caracteres")]
        public string Alias { get; set; }

        /// <summary>
        /// Fileira
        /// </summary>
        /// <example>A</example>
        [Display(Name = "Fileira da mesa.")]
        [Required(ErrorMessage = "O campo fileira da mesa é obrigatório.")]
        [EnumDataType(typeof(Fileira), ErrorMessage = "O campo fileira é inválida.")]
        public Fileira Fileira { get; set; }

        /// <summary>
        /// Coluna
        /// </summary>
        /// <example>1</example>
        [Display(Name = "Coluna da mesa.")]
        [Required(ErrorMessage = "O campo coluna da mesa é obrigatório.")]
        public int Coluna { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <example>Ativo</example>
        [Display(Name = "Status da mesa.")]
        [Required(ErrorMessage = "O campo status da mesa é obrigatório.")]
        [EnumDataType(typeof(Status), ErrorMessage = "O campo status é inválido.")]
        public Status Status { get; set; }

        /// <summary>
        /// Cadeiras
        /// </summary>
        [Display(Name = "Cadeiras da mesa.")]
        [Required(ErrorMessage = "O campo cadeiras da mesa é obrigatório.")]
        public List<ReferenciaCadeiraDto> Cadeiras { get; set; }
    }
}