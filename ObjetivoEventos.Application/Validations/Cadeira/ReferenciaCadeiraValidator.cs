using FluentValidation;
using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Cadeira
{
    public class ReferenciaCadeiraValidator : AbstractValidator<ReferenciaCadeiraDto>
    {
        private readonly ICadeiraApplication cadeiraApplication;

        public ReferenciaCadeiraValidator(ICadeiraApplication cadeiraApplication)
        {
            this.cadeiraApplication = cadeiraApplication;

            RuleFor(x => x.Id)
                .NotNull()
                .WithMessage("O campo id da cadeira não pode ser nulo.")

                .NotEmpty()
                .WithMessage("O campo id da cadeira não pode ser vazio.")

                .Must((dto, cancelar) =>
                {
                    return ValidarId(dto.Id);
                }).WithMessage("Nenhuma cadeira foi encontrada com o id informado.");
        }

        private bool ValidarId(Guid id)
        {
            return cadeiraApplication.ValidarId(id);
        }
    }
}