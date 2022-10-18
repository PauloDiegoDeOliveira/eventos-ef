using FluentValidation;
using ObjetivoEventos.Application.Dtos.Mesa;
using ObjetivoEventos.Application.Interfaces;
using ObjetivoEventos.Application.Validations.Cadeira;
using System.Linq;

namespace ObjetivoEventos.Application.Validations.Mesa
{
    public class PostMesaValidator : AbstractValidator<PostMesaDto>
    {
        private readonly IMesaApplication mesaApplication;

        public PostMesaValidator(IMesaApplication mesaApplication, ICadeiraApplication cadeiraApplication)
        {
            this.mesaApplication = mesaApplication;

            RuleFor(x => x.Nome)
             .NotNull()
             .WithMessage("O campo nome não pode ser nulo.")

             .NotEmpty()
             .WithMessage("O campo nome não pode ser vazio.");

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

            RuleFor(x => x.Cadeiras)
             .NotNull()
             .WithMessage("As cadeiras não podem ser nulas.")

             .NotEmpty()
             .WithMessage("As cadeiras não podem ser vazias.");

            When(x => x.Cadeiras.Count > 0 && x.Cadeiras != null, () =>
            {
                RuleFor(dto => dto)
                   .Must((dto, cancellation) =>
                   {
                       return ValidarInputListaCadeira(dto);
                   }).WithMessage("Não é possivel inserir cadeiras com o mesmo id em uma mesa.");

                RuleForEach(x => x.Cadeiras)
                   .SetValidator(new ReferenciaCadeiraValidator(cadeiraApplication));
            });

            When(x => !string.IsNullOrEmpty(x.Fileira.ToString()) && x.Coluna > 0, () =>
            {
                RuleFor(dto => dto)
                   .Must((dto, cancellation) =>
                   {
                       return !ValidarFileiraEColunaPost(dto);
                   }).WithMessage("Já existe uma mesa cadastrada com a fileira e coluna informados.");
            });
        }

        private static bool ValidarInputListaCadeira(PostMesaDto postMesaDto)
        {
            return postMesaDto.Cadeiras.GroupBy(x => x.Id).All(g => g.Count() == 1);
        }

        private bool ValidarFileiraEColunaPost(PostMesaDto postMesaDto)
        {
            return mesaApplication.ValidarFileiraEColunaPost(postMesaDto);
        }
    }
}