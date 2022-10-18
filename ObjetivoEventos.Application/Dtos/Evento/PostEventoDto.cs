using Microsoft.AspNetCore.Http;
using ObjetivoEventos.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Evento
{
    /// <summary>
    /// Objeto utilizado para inserção.
    /// </summary>
    public class PostEventoDto
    {
        [Display(Name = "Imagem do evento.")]
        [Required(ErrorMessage = "O campo imagem do evento é obrigatório.")]
        public IFormFile ImagemUpload { get; set; }

        /// <summary>
        /// Id do local
        /// </summary>
        /// <example>5C7AC53D-0AE7-41B8-CEC9-08DA91C2F232</example>
        [Display(Name = "Id do local.")]
        [Required(ErrorMessage = "O campo id do local é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id do local está em um formato inválido.")]
        public Guid LocalId { get; set; }

        /// <summary>
        /// Nome
        /// </summary>
        /// <example>Evento 1</example>
        [Display(Name = "Nome do evento.")]
        [Required(ErrorMessage = "O campo nome do evento é obrigatório.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "O campo nome do evento precisa ter entre 3 ou 100 caracteres")]
        public string Nome { get; set; }

        /// <summary>
        /// Sobre
        /// </summary>
        /// <example>Formatura 7º ano</example>
        [Display(Name = "Sobre o evento.")]
        public string Sobre { get; set; }

        /// <summary>
        /// Data
        /// </summary>
        /// <example>2022-08-27 09:34:07.4255806</example>
        [Display(Name = "Data do evento.")]
        [Required(ErrorMessage = "O campo data do evento é obrigatório.")]
        [DataType(DataType.DateTime, ErrorMessage = "O campo data do evento está em formato incorreto.")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DataEvento { get; set; }

        /// <summary>
        /// Duração em horas
        /// </summary>
        /// <example>10</example>
        [Display(Name = "Duração o evento.")]
        public string Duracao { get; set; }

        /// <summary>
        /// Cantor
        /// </summary>
        /// <example>Cantor 1</example>
        [Display(Name = "Cantor do evento.")]
        public string Cantor { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <example>Ativo</example>
        [Display(Name = "Status do evento.")]
        [Required(ErrorMessage = "O campo status do evento é obrigatório.")]
        [EnumDataType(typeof(Status), ErrorMessage = "O campo status é inválido.")]
        public Status Status { get; set; }
    }
}