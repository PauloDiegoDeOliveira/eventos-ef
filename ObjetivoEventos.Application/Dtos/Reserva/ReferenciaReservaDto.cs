using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Reserva
{
    /// <summary>
    /// Objeto utilizado para referências.
    /// </summary>
    public class ReferenciaReservaDto
    {
        /// <summary>
        /// Id da reserva
        /// </summary>
        /// <example>9F96BC14-07D4-4706-47E4-08DA85F3B504</example>
        [Display(Name = "Id da reserva.")]
        [Required(ErrorMessage = "O campo id da reserva é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id da reserva está em um formato inválido.")]
        public Guid Id { get; set; }
    }
}