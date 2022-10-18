using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Endereco
{
    /// <summary>
    /// Objeto utilizado para atualização.
    /// </summary>
    public class PutEnderecoDto : PostEnderecoDto
    {
        /// <summary>
        /// Id do Endereço
        /// </summary>
        /// <example>876777D9-EE18-4798-D3AE-08DA85F40146</example>
        [Display(Name = "Id do Endereço.")]
        [Required(ErrorMessage = "O campo id do endereço é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id do endereço está em um formato inválido.")]
        public Guid Id { get; set; }
    }
}