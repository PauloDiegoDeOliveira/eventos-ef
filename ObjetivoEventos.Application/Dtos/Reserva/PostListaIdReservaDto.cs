using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Reserva
{
    /// <summary>
    /// Objeto utilizado de parametro de consulta.
    /// </summary>
    public class PostListaIdReservaDto
    {        
        /// <summary>
        /// Ids de Reservas
        /// </summary>
        [Display(Name = "Ids de Reservas.")]
        [Required(ErrorMessage = "O campo reservas é obrigatório.")]
        public List<Guid> Ids { get; set; }
    }
}

