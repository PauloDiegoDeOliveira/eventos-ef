using FluentValidation;
using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Cadeira
{
    public class PutStatusCadeiraValidator : AbstractValidator<PutStatusCadeiraDto>
    {
        private readonly ICadeiraApplication cadeiraApplication;

        public PutStatusCadeiraValidator(ICadeiraApplication cadeiraApplication)
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
            return cadeiraApplication.ValidarId(id);
        }
    }
}