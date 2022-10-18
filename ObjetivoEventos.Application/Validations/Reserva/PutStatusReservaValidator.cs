using FluentValidation;
using ObjetivoEventos.Application.Dtos.Reserva;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Reserva
{
    public class PutStatusReservaValidator : AbstractValidator<PutStatusReservaDto>
    {
        private readonly IReservaApplication reservaApplication;

        public PutStatusReservaValidator(IReservaApplication reservaApplication)
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
            return reservaApplication.ValidarId(id);
        }
    }
}