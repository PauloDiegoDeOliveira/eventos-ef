using FluentValidation;
using ObjetivoEventos.Application.Dtos.Local;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Local
{
    public class PutStatusLocalValidator : AbstractValidator<PutStatusLocalDto>
    {
        private readonly ILocalApplication localApplication;

        public PutStatusLocalValidator(ILocalApplication localApplication)
        {
            this.localApplication = localApplication;

            RuleFor(x => x.Id)
               .NotNull()
               .WithMessage("O campo id do local não pode ser nulo.")

               .NotEmpty()
               .WithMessage("O campo id do local não pode ser vazio.")

               .Must((dto, cancelar) =>
               {
                   return ValidarId(dto.Id);
               }).WithMessage("Nenhum local foi encontrado com o id informado.");

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
            return localApplication.ValidarId(id);
        }
    }
}