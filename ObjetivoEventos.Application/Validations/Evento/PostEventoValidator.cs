using FluentValidation;
using ObjetivoEventos.Application.Dtos.Evento;
using ObjetivoEventos.Application.Interfaces;
using System;

namespace ObjetivoEventos.Application.Validations.Evento
{
    public class PostEventoValidator : AbstractValidator<PostEventoDto>
    {
        private readonly IEventoApplication eventoApplication;
        private readonly ILocalApplication localApplication;

        public PostEventoValidator(IEventoApplication eventoApplication,
                                   ILocalApplication localApplication)
        {
            this.eventoApplication = eventoApplication;
            this.localApplication = localApplication;

            RuleFor(x => x.LocalId)
               .NotNull()
               .WithMessage("O campo id do local não pode ser nulo.")

               .NotEmpty()
               .WithMessage("O campo id do local não pode ser vazio.")

               .Must((postLocalDto, cancelar) =>
               {
                   return ExisteLocalId(postLocalDto.LocalId);
               }).WithMessage("Nenhum local foi encontrado com o id informado.");

            RuleFor(x => x.Nome)
              .NotNull()
              .WithMessage("O campo nome não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo nome não pode ser vazio.");

            RuleFor(x => x.Status)
             .NotNull()
             .WithMessage("O campo status não pode ser nulo.")

             .NotEmpty()
             .WithMessage("O campo status não pode ser vazio.")

             .IsInEnum()
             .WithMessage("O valor do campo status não é valido.");

            When(dto => !string.IsNullOrEmpty(dto.DataEvento.ToString()), () =>
            {
                RuleFor(dto => dto)
                   .Must((dto, cancellation) =>
                   {
                       return !ValidarDataHoraPost(dto);
                   }).WithMessage("Já existe um evento cadastrado no mesmo local, data e hora informado.");
            });

            RuleFor(x => x.ImagemUpload)
              .NotEmpty()
              .WithMessage("Insira uma imagem!");

            When(x => x.ImagemUpload != null, () =>
            {
                RuleFor(m => m.ImagemUpload.Length)
                    .GreaterThan(0)
                    .WithMessage("A imagem enviada está corrompida.");

                RuleFor(x => x.ImagemUpload.Length)
                    .NotNull()
                    .NotEmpty()
                    .LessThanOrEqualTo(10996999)
                    .WithMessage("O tamanho da imagem é maior que o permitido (10 Mb)");

                RuleFor(x => x.ImagemUpload.ContentType)
                    .NotNull()
                    .NotEmpty()
                    .Must(x => x.Equals("image/jpeg") || x.Equals("image/jpg") || x.Equals("image/png"))
                    .WithMessage("Extensões permitidas jpg, jpeg e png.");
            });
        }

        private bool ValidarDataHoraPost(PostEventoDto postEventoDto)
        {
            return eventoApplication.ValidarDataHoraPost(postEventoDto);
        }

        private bool ExisteLocalId(Guid localId)
        {
            return localApplication.ExisteLocalId(localId);
        }
    }
}