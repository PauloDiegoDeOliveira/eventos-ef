using FluentValidation;
using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Application.Interfaces;

namespace ObjetivoEventos.Application.Validations.Cadeira
{
    public class PostCadeiraValidator : AbstractValidator<PostCadeiraDto>
    {
        private readonly ICadeiraApplication cadeiraApplication;

        public PostCadeiraValidator(ICadeiraApplication cadeiraApplication)
        {
            this.cadeiraApplication = cadeiraApplication;

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
                RuleFor(postCadeiraDto => postCadeiraDto)
                   .Must((postCadeiraDto, cancellation) =>
                   {
                       return !ValidarFileiraEColunaPost(postCadeiraDto);
                   }).WithMessage("Já existe uma cadeira cadastrada com a fileira e coluna informados.");
            });
        }

        private bool ValidarFileiraEColunaPost(PostCadeiraDto postCadeiraDto)
        {
            return cadeiraApplication.ValidarFileiraEColunaPost(postCadeiraDto);
        }
    }
}