using FluentValidation;
using ObjetivoEventos.Application.Dtos.Evento;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Evento
{
    public class PutStatusEventoValidator : AbstractValidator<PutStatusEventoDto>
    {
        private readonly IEventoApplication eventoApplication;

        public PutStatusEventoValidator(IEventoApplication eventoApplication)
        {
            this.eventoApplication = eventoApplication;

            RuleFor(x => x.Id)
               .NotNull()
               .WithMessage("O campo id do evento não pode ser nulo.")

               .NotEmpty()
               .WithMessage("O campo id do evento não pode ser vazio.")

               .Must((dto, cancelar) =>
               {
                   return ValidarId(dto.Id);
               }).WithMessage("Nenhum evento foi encontrado com o id informado.");

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
            return eventoApplication.ValidarId(id);
        }
    }
}