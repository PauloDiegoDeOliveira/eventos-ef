using ObjetivoEventos.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Reserva
{
    /// <summary>
    /// Objeto utilizado para inserção.
    /// </summary>
    public class PostReservaDto
    {
        /// <summary>
        /// Id do evento
        /// </summary>
        /// <example>658216E4-5CC1-4F97-ACCE-08DA91D54DFA</example>
        [Display(Name = "Id do evento.")]
        [Required(ErrorMessage = "O campo id do evento é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id do evento está em um formato inválido.")]
        public Guid EventoId { get; set; }

        /// <summary>
        /// Id do local
        /// </summary>
        /// <example>5C7AC53D-0AE7-41B8-CEC9-08DA91C2F232</example>
        [Display(Name = "Id do local.")]
        [Required(ErrorMessage = "O campo id do local é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id do local está em um formato inválido.")]
        public Guid LocalId { get; set; }

        /// <summary>
        /// Id do setor
        /// </summary>
        /// <example>0B28EA5F-1BA2-48FB-471B-08DA91D58F96</example>
        [Display(Name = "Id do setor.")]
        [Required(ErrorMessage = "O campo id do setor é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id do setor está em um formato inválido.")]
        public Guid SetorId { get; set; }

        /// <summary>
        /// Id da mesa
        /// </summary>
        /// <example>CF86D724-64EE-472B-3C8C-08DA8C424929</example>
        [Display(Name = "Id da mesa.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id da mesa está em um formato inválido.")]
        public Guid? MesaId { get; set; }

        /// <summary>
        /// Id da cadeira
        /// </summary>
        /// <example>C99EFDCD-8069-4C8B-118C-08DA91C15290</example>
        [Display(Name = "Id da cadeira.")]
        [Required(ErrorMessage = "O campo id da cadeira é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id da cadeira está em um formato inválido.")]
        public Guid CadeiraId { get; set; }

        /// <summary>
        /// Id da connection - SignalR
        /// </summary>
        /// <example>laM-t8bZdmcvCUroTBYupw</example>
        [Display(Name = "Id da connection - SignalR.")]
        [Required(ErrorMessage = "O campo id da connection é obrigatório.")]
        public string ConnectionId { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        /// <example>Ativo</example>
        [Display(Name = "Status da reserva.")]
        [Required(ErrorMessage = "O campo status da reserva é obrigatório.")]
        [EnumDataType(typeof(Status), ErrorMessage = "O campo status é inválido.")]
        public Status Status { get; set; }
    }
}