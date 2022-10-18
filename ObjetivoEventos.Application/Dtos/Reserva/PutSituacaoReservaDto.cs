using ObjetivoEventos.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Reserva
{
    /// <summary>
    /// Objeto utilizado para alteração de situação reserva.
    /// </summary>
    public class PutSituacaoReservaDto
    {
        /// <summary>
        /// Ids
        /// </summary>
        /// <example>18708800-31FC-4FBA-CEC1-08DA90081621</example>
        [Display(Name = "Lista de ids de reservas.")]
        [Required(ErrorMessage = "O campo ids das reservas é obrigatório.")]
        public List<Guid> Ids { get; set; }

        /// <summary>
        /// Situação
        /// </summary>
        /// <example>Reservado</example>
        [Display(Name = "Situação da Reserva.")]
        [Required(ErrorMessage = "O campo situação da reserva é obrigatório.")]
        [EnumDataType(typeof(SituacaoReserva), ErrorMessage = "O campo situação reserva é inválido.")]
        public SituacaoReserva SituacaoReserva { get; set; }

        public PutSituacaoReservaDto()
        { }

        public PutSituacaoReservaDto(List<Guid> ids, SituacaoReserva situacaoReserva)
        {
            Ids = ids;
            SituacaoReserva = situacaoReserva;
        }
    }
}