using FluentValidation;
using ObjetivoEventos.Application.Dtos.Reserva;
using ObjetivoEventos.Application.Interfaces;
using System;
using System.Collections.Generic;

namespace ObjetivoEventos.Application.Validations.Reserva
{
    public class PutSituacaoReservaValidator : AbstractValidator<PutSituacaoReservaDto>
    {
        private readonly IReservaApplication reservaApplication;

        public PutSituacaoReservaValidator(IReservaApplication reservaApplication)
        {
            this.reservaApplication = reservaApplication;

            RuleFor(x => x.Ids)
                .NotNull()
                .WithMessage("A lista de ids não pode ser nulo.")

                .NotEmpty()
                .WithMessage("A lista de ids não pode ser vaio.");

            RuleFor(x => x.SituacaoReserva)
                 .NotNull()
                 .WithMessage("O campo situação reserva não pode ser nulo.")

                 .NotEmpty()
                 .WithMessage("O campo situação reserva não pode ser vazio.")

                 .IsInEnum()
                 .WithMessage("O valor do campo situação reserva não é valido.");

            When(x => x.Ids != null && x.Ids.Count > 0, () =>
            {
                RuleFor(dto => dto.Ids)
                   .Must((dto, cancellation) =>
                   {
                       return ValidaListId(dto.Ids);
                   }).WithMessage("Reserva não encontrada com o id informado.");
            });
        }

        public bool ValidaListId(List<Guid> ids)
        {
            return reservaApplication.ValidaListId(ids);
        }
    }
}