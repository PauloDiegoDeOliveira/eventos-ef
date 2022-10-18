using FluentValidation;
using ObjetivoEventos.Application.Dtos.Local;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Local
{
    public class PutLocalValidator : AbstractValidator<PutLocalDto>
    {
        private readonly ILocalApplication localApplication;

        public PutLocalValidator(ILocalApplication localApplication)
        {
            this.localApplication = localApplication;

            RuleFor(x => x.Id)
               .NotNull()
               .WithMessage("O campo id do local não pode ser nulo.")

               .NotEmpty()
               .WithMessage("O campo id do local não pode ser vazio.")

               .Must((putCadeiraDto, cancelar) =>
               {
                   return ValidarId(putCadeiraDto.Id);
               }).WithMessage("Nenhum local foi encontrado com o id informado.");

            RuleFor(x => x.Status)
             .NotNull()
             .WithMessage("O campo status não pode ser nulo.")

             .NotEmpty()
             .WithMessage("O campo status não pode ser vazio.")

             .IsInEnum()
             .WithMessage("O valor do campo status não é valido.");

            When(x => !string.IsNullOrEmpty(x.Nome), () =>
            {
                RuleFor(dto => dto)
                   .Must((dto, cancellation) =>
                   {
                       return !ValidarNomePut(dto);
                   }).WithMessage("Já existe um local cadastrado com o nome informado.");
            });
        }

        private bool ValidarId(Guid id)
        {
            return localApplication.ValidarId(id);
        }

        private bool ValidarNomePut(PutLocalDto putLocalDto)
        {
            return localApplication.ValidarNomePut(putLocalDto);
        }
    }
}