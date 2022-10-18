using FluentValidation;
using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Cadeira
{
    public class PutCadeiraValidator : AbstractValidator<PutCadeiraDto>
    {
        private readonly ICadeiraApplication cadeiraApplication;

        public PutCadeiraValidator(ICadeiraApplication cadeiraApplication)
        {
            this.cadeiraApplication = cadeiraApplication;

            RuleFor(x => x.Id)
               .NotNull()
               .WithMessage("O campo id da cadeira não pode ser nulo.")

               .NotEmpty()
               .WithMessage("O campo id da cadeira não pode ser vazio.")

               .Must((putCadeiraDto, cancelar) =>
               {
                   return ValidarId(putCadeiraDto.Id);
               }).WithMessage("Nenhuma cadeira foi encontrada com o id informado.");

            RuleFor(x => x.Fileira)
              .NotNull()
              .WithMessage("O campo fileira não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo fileira não pode ser vazio.")

              .IsInEnum()
              .WithMessage("O valor do campo fileira não é valido.");

            RuleFor(x => x.Coluna)
              .NotNull()
              .WithMessage("O campo coluna não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo coluna não pode ser vazio.");

            RuleFor(x => x.Status)
             .NotNull()
             .WithMessage("O campo status não pode ser nulo.")

             .NotEmpty()
             .WithMessage("O campo status não pode ser vazio.")

             .IsInEnum()
             .WithMessage("O valor do campo status não é valido.");

            When(x => !string.IsNullOrEmpty(x.Fileira.ToString()) && x.Coluna > 0, () =>
            {
                RuleFor(dto => dto)
                   .Must((dto, cancellation) =>
                   {
                       return !ValidarFileiraEColunaPut(dto);
                   }).WithMessage("Já existe uma cadeira cadastrada com a fileira e coluna informados.");
            });
        }

        private bool ValidarId(Guid id)
        {
            return cadeiraApplication.ValidarId(id);
        }

        private bool ValidarFileiraEColunaPut(PutCadeiraDto putCadeiraDto)
        {
            return cadeiraApplication.ValidarFileiraEColunaPut(putCadeiraDto);
        }
    }
}