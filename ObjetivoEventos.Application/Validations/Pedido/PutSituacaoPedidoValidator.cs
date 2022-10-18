using FluentValidation;
using ObjetivoEventos.Application.Dtos.Pedido;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Pedido
{
    public class PutSituacaoPedidoValidator : AbstractValidator<PutSituacaoPedidoDto>
    {
        private readonly IPedidoApplication pedidoApplication;

        public PutSituacaoPedidoValidator(IPedidoApplication pedidoApplication)
        {
            this.pedidoApplication = pedidoApplication;

            RuleFor(x => x.Id)
              .NotNull()
              .WithMessage("O campo id do pedido não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo id do pedido não pode ser vazio.")

               .Must((dto, cancelar) =>
               {
                   return ValidarPedidoId(dto.Id);
               }).WithMessage("Nenhum pedido foi encontrado com o id informado.");

            RuleFor(x => x.SituacaoPedido)
                 .NotNull()
                 .WithMessage("O campo situação pedido não pode ser nulo.")

                 .NotEmpty()
                 .WithMessage("O campo situação pedido não pode ser vazio.")

                 .IsInEnum()
                 .WithMessage("O valor do campo situação pedido não é valido.");
        }

        public bool ValidarPedidoId(Guid id)
        {
            return pedidoApplication.ValidarId(id);
        }
    }
}