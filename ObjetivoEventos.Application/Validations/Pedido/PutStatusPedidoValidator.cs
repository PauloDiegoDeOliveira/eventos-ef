using FluentValidation;
using ObjetivoEventos.Application.Dtos.Pedido;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Pedido
{
    public class PutStatusPedidoValidator : AbstractValidator<PutStatusPedidoDto>
    {
        private readonly IPedidoApplication pedidoApplication;

        public PutStatusPedidoValidator(IPedidoApplication pedidoApplication)
        {
            this.pedidoApplication = pedidoApplication;

            RuleFor(x => x.Id)
               .NotNull()
               .WithMessage("O campo id do pedido não pode ser nulo.")

               .NotEmpty()
               .WithMessage("O campo id do pedido não pode ser vazio.")

               .Must((dto, cancelar) =>
               {
                   return ValidarId(dto.Id);
               }).WithMessage("Nenhum pedido foi encontrado com o id informado.");

            RuleFor(x => x.Status)
             .NotNull()
             .WithMessage("O campo status não pode ser nulo.")

             .NotEmpty()
             .WithMessage("O campo status não pode ser vazio.")

             .IsInEnum()
             .WithMessage("O valor do campo status não é valido.");
        }

        private bool ValidarId(Guid id)
        {
            return pedidoApplication.ValidarId(id);
        }
    }
}