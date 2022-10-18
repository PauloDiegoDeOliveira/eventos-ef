using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Parameters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Setor
{
    /// <summary>
    /// Objeto utilizado para inserção.
    /// </summary>
    public class PostAutomaticoSetorDto
    {
        /// <summary>
        /// Id do local
        /// </summary>
        /// <example>876777D9-EE18-4798-D3AE-08DA85F40146</example>
        [Display(Name = "Id do local.")]
        [Required(ErrorMessage = "O campo id do local é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id do local está em um formato inválido.")]
        public Guid LocalId { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        /// <example>Setor 1</example>
        [Display(Name = "Nome do setor.")]
        [Required(ErrorMessage = "O campo nome do setor é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo nome do setor precisa ter entre 3 ou 100 caracteres")]
        public string Nome { get; set; }

        /// <summary>
        /// Preço
        /// </summary>
        /// <example>100</example>
        [Display(Name = "Preço do setor.")]
        [Required(ErrorMessage = "O campo preço do setor é obrigatório.")]
        public float Preco { get; set; }

        /// <summary>
        /// Posição
        /// </summary>
        /// <example>main1</example>
        [Display(Name = "Posição do setor.")]
        [Required(ErrorMessage = "O campo nome da posição é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo nome da posição precisa ter entre 3 ou 100 caracteres")]
        public string Posicao { get; set; }

        /// <summary>
        /// Alias
        /// </summary>
        /// <example>main1</example>
        [Display(Name = "Alias do setor.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo alias precisa ter entre 3 ou 100 caracteres")]
        public string Alias { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <example>Ativo</example>
        [Display(Name = "Status do setor.")]
        [Required(ErrorMessage = "O campo status do setor é obrigatório.")]
        [EnumDataType(typeof(Status), ErrorMessage = "O campo status é inválido.")]
        public Status Status { get; set; }

        [Display(Name = "Grid do setor.")]
        public List<GridParameters> GridParameters { get; set; }
    }
}