using ObjetivoEventos.Application.Attributes;
using ObjetivoEventos.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Base
{
    /// <summary>
    /// Objeto utilizado para alteração de status.
    /// </summary>
    public class PutStatusDto
    {
        /// <summary>
        /// Id
        /// </summary>
        /// <example>876777D9-EE18-4798-D3AE-08DA85F40146</example>
        [Display(Name = "Id.")]
        [Required(ErrorMessage = "O campo id é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O Id está em um formato inválido.")]
        [ValidaGuid(ErrorMessage = "O campo id não pode ser nulo ou vazio.")]
        public Guid Id { get; set; }

        /// <summary>
        /// Status para qual será alterado
        /// </summary>
        /// <example>Ativo</example>
        [Display(Name = "Status.")]
        [Required(ErrorMessage = "O campo status é obrigatório.")]
        [EnumDataType(typeof(Status))]
        public Status Status { get; set; }

        public PutStatusDto()
        {
        }

        public PutStatusDto(Guid id, Status status)
        {
            Id = id;
            Status = status;
        }
    }
}