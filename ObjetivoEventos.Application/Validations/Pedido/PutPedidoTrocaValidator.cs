using FluentValidation;
using ObjetivoEventos.Application.Dtos.Pedido;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Pedido
{
    public class PutPedidoTrocaValidator : AbstractValidator<PutPedidoTrocaDto>
    {
        private readonly IReservaApplication reservaApplication;
        private readonly IPedidoApplication pedidoApplication;

        public PutPedidoTrocaValidator(IReservaApplication reservaApplication, IPedidoApplication pedidoApplication)
        {
            this.pedidoApplication = pedidoApplication;
            this.reservaApplication = reservaApplication;

            RuleFor(x => x.PedidoId)
              .NotNull()
              .WithMessage("O campo id do pedido não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo id do pedido não pode ser vazio.")

               .Must((dto, cancelar) =>
               {
                   return ValidarPedidoId(dto.PedidoId);
               }).WithMessage("Nenhum pedido foi encontrado com o id informado.");

            RuleFor(x => x.ReservaAntiga)
              .NotNull()
              .WithMessage("O campo id da reserva antiga não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo id da reserva antiga não pode ser vazio.")

               .Must((dto, cancelar) =>
               {
                   return ValidarReservaId(dto.ReservaAntiga);
               }).WithMessage("Nenhuma reserva foi encontrada com o id de reserva antiga.");

            RuleFor(x => x.NovaReserva)
              .NotNull()
              .WithMessage("O campo id da nova reserva  não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo id da nova reserva não pode ser vazio.")

               .Must((dto, cancelar) =>
               {
                   return ValidarReservaId(dto.NovaReserva);
               }).WithMessage("Nenhuma reserva foi encontrada com o id de nova reserva.");
        }

        public bool ValidarPedidoId(Guid id)
        {
            return pedidoApplication.ValidarId(id);
        }

        public bool ValidarReservaId(Guid id)
        {
            return reservaApplication.ValidarId(id);
        }
    }
}