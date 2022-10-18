using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Cadeira
{
    /// <summary>
    /// Objeto utilizado para referências.
    /// </summary>
    public class ReferenciaCadeiraDto
    {
        /// <summary>
        /// Id da cadeira
        /// </summary>
        /// <example>C99EFDCD-8069-4C8B-118C-08DA91C15290</example>
        [Display(Name = "Id da cadeira.")]
        [Required(ErrorMessage = "O campo id da cadeira é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id da cadeira está em um formato inválido.")]
        public Guid Id { get; set; }
    }
}