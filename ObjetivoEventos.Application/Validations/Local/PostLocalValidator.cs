using FluentValidation;
using ObjetivoEventos.Application.Dtos.Local;
using ObjetivoEventos.Application.Interfaces;

namespace ObjetivoEventos.Application.Validations.Local
{
    public class PostLocalValidator : AbstractValidator<PostLocalDto>
    {
        private readonly ILocalApplication localApplication;

        public PostLocalValidator(ILocalApplication localApplication)
        {
            this.localApplication = localApplication;

            RuleFor(x => x.Nome)
              .NotNull()
              .WithMessage("O campo nome não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo nome não pode ser vazio.");

            RuleFor(x => x.CEP)
             .NotNull()
             .WithMessage("O campo CEP não pode ser nulo.")

             .NotEmpty()
             .WithMessage("O campo CEP não pode ser vazio.");

            RuleFor(x => x.Status)
             .NotNull()
             .WithMessage("O campo status não pode ser nulo.")

             .NotEmpty()
             .WithMessage("O campo status não pode ser vazio.")

             .IsInEnum()
             .WithMessage("O valor do campo status não é valido.");

            When(x => !string.IsNullOrEmpty(x.Nome), () =>
            {
                RuleFor(postCadeiraDto => postCadeiraDto)
                   .Must((postCadeiraDto, cancellation) =>
                   {
                       return !ValidarNomePost(postCadeiraDto);
                   }).WithMessage("Já existe um local cadastrado com o nome informado.");
            });
        }

        private bool ValidarNomePost(PostLocalDto postLocalDto)
        {
            return localApplication.ValidarNomePost(postLocalDto);
        }
    }
}