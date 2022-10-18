using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Cadeira
{
    /// <summary>
    /// Objeto utilizado para atualização.
    /// </summary>
    public class PutCadeiraDto : PostCadeiraDto
    {
        /// <summary>
        /// Id da Cadeira
        /// </summary>
        /// <example>876777D9-EE18-4798-D3AE-08DA85F40146</example>
        [Display(Name = "Id da cadeira.")]
        [Required(ErrorMessage = "O campo id da cadeira é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id da cadeira está em um formato inválido.")]
        public Guid Id { get; set; }
    }
}