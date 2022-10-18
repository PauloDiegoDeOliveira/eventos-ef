using ObjetivoEventos.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Domain.Parameters
{
    public class GridParameters
    {
        /// <summary>
        /// Fileira
        /// </summary>
        /// <example>A</example>
        [Display(Name = "Fileira da cadeira.")]
        [Required(ErrorMessage = "Fileira da cadeira é obrigatório.")]
        [EnumDataType(typeof(Fileira))]
        public Fileira Fileira { get; set; }

        /// <summary>
        /// Coluna
        /// </summary>
        /// <example>20</example>
        [Display(Name = "Coluna da cadeira.")]
        [Required(ErrorMessage = "Coluna da cadeira é obrigatório.")]
        [Range(1, 1000, ErrorMessage = "O  valor de coluna deve estar entre {1} e {2}.")]
        public int Coluna { get; set; }
    }
}