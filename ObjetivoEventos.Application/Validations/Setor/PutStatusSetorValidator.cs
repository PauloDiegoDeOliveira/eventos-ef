using FluentValidation;
using ObjetivoEventos.Application.Dtos.Setor;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Setor
{
    public class PutStatusSetorValidator : AbstractValidator<PutStatusSetorDto>
    {
        private readonly ISetorApplication setorApplication;

        public PutStatusSetorValidator(ISetorApplication setorApplication)
        {
            this.setorApplication = setorApplication;

            RuleFor(x => x.Id)
               .NotNull()
               .WithMessage("O campo id do setor não pode ser nulo.")

               .NotEmpty()
               .WithMessage("O campo id do setor não pode ser vazio.")

               .Must((dto, cancelar) =>
               {
                   return ValidarId(dto.Id);
               }).WithMessage("Nenhum setor foi encontrado com o id informado.");

            RuleFor(x => x.Status)
             .NotNull()
             .WithMessage("O status não pode ser nulo.")

             .NotEmpty()
             .WithMessage("O status não pode ser vazio.")

             .IsInEnum()
             .WithMessage("O valor do campo status não é valido.");
        }

        private bool ValidarId(Guid id)
        {
            return setorApplication.ValidarId(id);
        }
    }
}