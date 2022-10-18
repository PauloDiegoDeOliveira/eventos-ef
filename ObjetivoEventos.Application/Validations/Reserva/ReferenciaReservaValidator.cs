using FluentValidation;
using ObjetivoEventos.Application.Dtos.Reserva;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Reserva
{
    public class ReferenciaReservaValidator : AbstractValidator<ReferenciaReservaDto>
    {
        private readonly IReservaApplication reservaApplication;

        public ReferenciaReservaValidator(IReservaApplication reservaApplication)
        {
            this.reservaApplication = reservaApplication;

            RuleFor(x => x.Id)
              .NotNull()
              .WithMessage("O campo id da reserva não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo id da reserva não pode ser vazio.")

              .Must((dto, cancelar) =>
              {
                  return ValidarId(dto.Id);
              }).WithMessage("Nenhuma reserva foi encontrada com o id informado.");
        }

        private bool ValidarId(Guid id)
        {
            return reservaApplication.ValidarId(id);
        }
    }
}