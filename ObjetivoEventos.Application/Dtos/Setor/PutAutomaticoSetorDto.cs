using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Setor
{
    /// <summary>
    /// Objeto utilizado para atualização.
    /// </summary>
    public class PutAutomaticoSetorDto : PostAutomaticoSetorDto
    {
        /// <summary>
        /// Id do setor
        /// </summary>
        /// <example>876777D9-EE18-4798-D3AE-08DA85F40146</example>
        [Display(Name = "Id do Setor.")]
        [Required(ErrorMessage = "O campo id do setor é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id do setor está em um formato inválido.")]
        public Guid Id { get; set; }
    }
}