using FluentValidation;
using ObjetivoEventos.Application.Dtos.Reserva;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Reserva
{
    public class PostReservaValidator : AbstractValidator<PostReservaDto>
    {
        private readonly IReservaApplication reservaApplication;
        private readonly ISetorApplication setorApplication;
        private readonly IMesaApplication mesaApplication;
        private readonly ICadeiraApplication cadeiraApplication;
        private readonly ILocalApplication localApplication;
        private readonly IEventoApplication eventoApplication;

        public PostReservaValidator(IReservaApplication reservaApplication, IEventoApplication eventoApplication,
            ILocalApplication localApplication,
            ICadeiraApplication cadeiraApplication,
            IMesaApplication mesaApplication,
            ISetorApplication setorApplication
            )
        {
            this.reservaApplication = reservaApplication;
            this.eventoApplication = eventoApplication;
            this.localApplication = localApplication;
            this.cadeiraApplication = cadeiraApplication;
            this.mesaApplication = mesaApplication;
            this.setorApplication = setorApplication;

            RuleFor(x => x.ConnectionId)
                .NotNull()
                .NotEmpty()
                .WithMessage("O campo id da conexão não pode ser null. Erro: 423");

            RuleFor(x => x.EventoId)
                .NotNull()
                .WithMessage("O campo id do evento não pode ser nulo.")

                .NotEmpty()
                .WithMessage("O campo id do evento não pode ser vazio.");

            RuleFor(x => x.LocalId)
                .NotNull()
                .WithMessage("O campo id do local não pode ser nulo.")

                .NotEmpty()
                .WithMessage("O campo id do local não pode ser vazio.");

            RuleFor(x => x.SetorId)
                .NotNull()
                .WithMessage("O campo id do setor não pode ser nulo.")

                .NotEmpty()
                .WithMessage("O campo id do setor não pode ser vazio.");

            RuleFor(x => x.CadeiraId)
                .NotNull()
                .WithMessage("O campo id da cadeira não pode ser nulo.")

                .NotEmpty()
                .WithMessage("O campo id da cadeira não pode ser vaio.");

            RuleFor(x => x.Status)
             .NotNull()
             .WithMessage("O campo status não pode ser nulo.")

             .NotEmpty()
             .WithMessage("O campo status não pode ser vazio.")

             .IsInEnum()
             .WithMessage("O valor do campo status não é valido.");

            When(x => x.LocalId != Guid.Empty, () =>
            {
                RuleFor(dto => dto.LocalId)
                   .Must((dto, cancellation) =>
                   {
                       return ExisteLocalId(dto.LocalId);
                   }).WithMessage("Nenhum local foi encontrado com o id informado.");
            });

            When(x => x.EventoId != Guid.Empty, () =>
            {
                RuleFor(dto => dto.EventoId)
                   .Must((dto, cancellation) =>
                   {
                       return ExisteEventoId(dto.EventoId);
                   }).WithMessage("Nenhum evento foi encontrado com o id informado.");
            });

            When(x => x.SetorId != Guid.Empty, () =>
            {
                RuleFor(dto => dto.SetorId)
                   .Must((dto, cancellation) =>
                   {
                       return ExisteSetorId(dto.SetorId);
                   }).WithMessage("Nenhum setor foi encontrado com o id informado.");
            });

            When(x => x.MesaId != Guid.Empty && x.MesaId != null, () =>
            {
                RuleFor(dto => dto.MesaId)
                   .Must((dto, cancellation) =>
                   {
                       return ExisteMesaId(dto.MesaId);
                   }).WithMessage("Nenhum mesa foi encontrado com o id informado.");
            });

            When(x => x.CadeiraId != Guid.Empty, () =>
            {
                RuleFor(dto => dto.CadeiraId)
                   .Must((dto, cancellation) =>
                   {
                       return ExisteCadeiraId(dto.CadeiraId);
                   }).WithMessage("Nenhum cadeira foi encontrado com o id informado.");
            });

            RuleFor(dto => dto)
                .Must((dto, cancellation) =>
                {
                    return !VerificaDisponibilidadeReserva(dto);
                }).WithMessage("Já existe uma reserva cadastrada com os dados informados.");

            RuleFor(x => x)
              .Must((dto, cancellation) =>
              {
                  return !ValidaQuantidadeCadeiras(dto);
              }).WithMessage("Só é permitido reservar 04 cadeiras por usuário.");

            RuleFor(dto => dto)
                   .Must((dto, cancellation) =>
                   {
                       return VerificaSetorCadeiraMesa(dto);
                   }).WithMessage("Os dados informados para o assento não existem no setor.");
        }

        private bool ExisteLocalId(Guid localId)
        {
            return localApplication.ExisteLocalId(localId);
        }

        private bool ExisteEventoId(Guid eventoId)
        {
            return eventoApplication.ValidarId(eventoId);
        }

        private bool ExisteSetorId(Guid setorId)
        {
            return setorApplication.ValidarId(setorId);
        }

        private bool ExisteMesaId(Guid? mesaId)
        {
            if (mesaId == null)
                return false;

            Guid guid = mesaId.Value;
            return mesaApplication.ValidarId(guid);
        }

        private bool ExisteCadeiraId(Guid cadeiraId)
        {
            return cadeiraApplication.ValidarId(cadeiraId);
        }

        private bool VerificaSetorCadeiraMesa(PostReservaDto postReservaDto)
        {
            return reservaApplication.ValidarSetorCadeiraMesa(postReservaDto);
        }

        private bool VerificaDisponibilidadeReserva(PostReservaDto postReservaDto)
        {
            return reservaApplication.VerificaDisponibilidadeReserva(postReservaDto);
        }

        private bool ValidaQuantidadeCadeiras(PostReservaDto postReservaDto)
        {
            return reservaApplication.ValidaQuantidadeCadeiras(postReservaDto);
        }
    }
}