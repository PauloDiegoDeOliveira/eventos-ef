using FluentValidation;
using ObjetivoEventos.Application.Dtos.Mesa;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Mesa
{
    public class PutStatusMesaValidator : AbstractValidator<PutStatusMesaDto>
    {
        private readonly IMesaApplication mesaApplication;

        public PutStatusMesaValidator(IMesaApplication mesaApplication)
        {
            this.mesaApplication = mesaApplication;

            RuleFor(x => x.Id)
               .NotNull()
               .WithMessage("O campo id da mesa não pode ser nulo.")

               .NotEmpty()
               .WithMessage("O campo id da mesa não pode ser vazio.")

               .Must((dto, cancelar) =>
               {
                   return ValidarId(dto.Id);
               }).WithMessage("Nenhuma mesa foi encontrada com o id informado.");

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
            return mesaApplication.ValidarId(id);
        }
    }
}