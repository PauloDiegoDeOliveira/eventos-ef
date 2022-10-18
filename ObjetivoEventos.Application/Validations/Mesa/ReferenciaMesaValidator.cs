using FluentValidation;
using ObjetivoEventos.Application.Dtos.Mesa;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Mesa
{
    public class ReferenciaMesaValidator : AbstractValidator<ReferenciaMesaDto>
    {
        private readonly IMesaApplication mesaApplication;

        public ReferenciaMesaValidator(IMesaApplication mesaApplication)
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
        }

        private bool ValidarId(Guid id)
        {
            return mesaApplication.ValidarId(id);
        }
    }
}