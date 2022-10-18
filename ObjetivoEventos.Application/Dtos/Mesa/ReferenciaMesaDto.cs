using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Mesa
{
    /// <summary>
    /// Objeto utilizado para referências.
    /// </summary>
    public class ReferenciaMesaDto
    {
        /// <summary>
        /// Id da mesa
        /// </summary>
        /// <example>CF86D724-64EE-472B-3C8C-08DA8C424929</example>
        [Display(Name = "Id da mesa.")]
        [Required(ErrorMessage = "O campo id da mesa é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id da mesa está em um formato inválido.")]
        public Guid Id { get; set; }
    }
}