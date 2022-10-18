using ObjetivoEventos.Application.Attributes;
using ObjetivoEventos.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ObjetivoEventos.Application.Dtos.Pedido
{
    /// <summary>
    /// Objeto utilizado para alteração de situação pedido.
    /// </summary>
    public class PutSituacaoPedidoDto
    {
        /// <summary>
        /// Ids
        /// </summary>
        /// <example>18708800-31FC-4FBA-CEC1-08DA90081621</example>
        [Display(Name = "Id do Pedido.")]
        [Required(ErrorMessage = "O campo id do pedido é obrigatório.")]
        [RegularExpression(@"^[{]?[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}[}]?$", ErrorMessage = "O campo id do pedido está em um formato inválido.")]
        [ValidaGuid(ErrorMessage = "O campo id do pedido não pode ser nulo ou vazio.")]
        public Guid Id { get; set; }

        /// <summary>
        /// Situação para qual o pedido será atualizado
        /// </summary>
        /// <example>AguardandoPagamento</example>
        [Display(Name = "Situação do Pedido.")]
        [Required(ErrorMessage = "O campo situação do pedido é obrigatório.")]
        [EnumDataType(typeof(SituacaoPedido), ErrorMessage = "O campo situção do pedido é inválido.")]
        public SituacaoPedido SituacaoPedido { get; set; }

        public PutSituacaoPedidoDto()
        { }

        public PutSituacaoPedidoDto(Guid id, SituacaoPedido situacaoPedido)
        {
            Id = id;
            SituacaoPedido = situacaoPedido;
        }
    }
}