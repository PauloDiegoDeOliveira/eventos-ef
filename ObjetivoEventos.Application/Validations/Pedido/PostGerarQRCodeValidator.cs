using FluentValidation;
using ObjetivoEventos.Application.Dtos.Pedido;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Pedido
{
    public class PostGerarQRCodeValidator : AbstractValidator<PostQrCodePedidoDto>
    {
        private readonly IPedidoApplication pedidoApplication;

        public PostGerarQRCodeValidator(IPedidoApplication pedidoApplication)
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
        }

        public bool ValidarId(Guid id)
        {
            return pedidoApplication.ValidarId(id);
        }
    }
}