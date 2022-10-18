using FluentValidation;
using ObjetivoEventos.Application.Dtos.Pedido;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Pedido
{
    public class PutValidaQrCodePedidoValidator : AbstractValidator<PutValidaQrCodePedidoDto>
    {
        private readonly IReservaApplication reservaApplication;
        private readonly IPedidoApplication pedidoApplication;
        private readonly IUsuarioApplication usuarioApplication;

        public PutValidaQrCodePedidoValidator(IReservaApplication reservaApplication, IPedidoApplication pedidoApplication, IUsuarioApplication usuarioApplication)
        {
            this.pedidoApplication = pedidoApplication;
            this.reservaApplication = reservaApplication;
            this.usuarioApplication = usuarioApplication;

            RuleFor(x => x.PedidoId)
              .NotNull()
              .WithMessage("O campo id do pedido não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo id do pedido não pode ser vazio.")

               .Must((dto, cancelar) =>
               {
                   return ValidarPedidoId(dto.PedidoId);
               }).WithMessage("Nenhum pedido foi encontrado com o id informado.");

            RuleFor(x => x.ReservaId)
              .NotNull()
              .WithMessage("O campo id do reserva não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo id do reserva não pode ser vazio.")

               .Must((dto, cancelar) =>
               {
                   return ValidarReservaId(dto.ReservaId);
               }).WithMessage("Nenhuma reserva foi encontrada com o id informado.");

            RuleFor(x => x.UsuarioId)
              .NotNull()
              .WithMessage("O campo id do usuário não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo id do usuário não pode ser vazio.")

               .Must((dto, cancelar) =>
               {
                   return ValidarUsuarioId(dto.UsuarioId);
               }).WithMessage("Nenhum usuário foi encontrado com o id informado.");
        }

        public bool ValidarPedidoId(Guid id)
        {
            return pedidoApplication.ValidarId(id);
        }

        public bool ValidarReservaId(Guid id)
        {
            return reservaApplication.ValidarId(id);
        }

        public bool ValidarUsuarioId(Guid id)
        {
            return usuarioApplication.ValidarId(id);
        }
    }
}