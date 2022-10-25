using ObjetivoEventos.Application.Dtos.Reserva;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Setor
{
    /// <summary>
    /// Objeto utilizado de parametro para visualização.
    /// </summary>
    public class PostViewDadosSetorDto
    {
        /// <summary>
        /// Reservas do pedido
        /// </summary>
        [Display(Name = "Reservas do pedido.")]
        [Required(ErrorMessage = "O campo reservas do pedido é obrigatório.")]
        public List<Guid> ReservasIds { get; set; }
    }
}