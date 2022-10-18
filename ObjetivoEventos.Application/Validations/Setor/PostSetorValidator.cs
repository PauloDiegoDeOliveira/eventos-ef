using FluentValidation;
using ObjetivoEventos.Application.Dtos.Setor;
using ObjetivoEventos.Application.Interfaces;
using ObjetivoEventos.Application.Validations.Cadeira;
using ObjetivoEventos.Application.Validations.Mesa;
using System;
using System.Linq;

namespace ObjetivoEventos.Application.Validations.Setor
{
    public class PostSetorValidator : AbstractValidator<PostSetorDto>
    {
        private readonly ILocalApplication localApplication;

        public PostSetorValidator(ISetorApplication setorApplication,
                                  ICadeiraApplication cadeiraApplication,
                                  IMesaApplication mesaApplication,
                                  ILocalApplication localApplication)
        {
            this.localApplication = localApplication;

            RuleFor(x => x.LocalId)
             .NotNull()
             .WithMessage("O campo id do local não pode ser nulo.")

             .NotEmpty()
             .WithMessage("O campo id do local não pode ser vazio.")

             .Must((dto, cancelar) =>
             {
                 return ExisteLocalId(dto.LocalId);
             }).WithMessage("Nenhum local foi encontrado com o id informado.");

            RuleFor(x => x.Nome)
              .NotNull()
              .WithMessage("O campo nome não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo nome não pode ser vazio.");

            RuleFor(x => x.Preco)
              .NotNull()
              .WithMessage("O campo preço não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo preço não pode ser vazio.");

            RuleFor(x => x.Posicao)
              .NotNull()
              .WithMessage("O campo posição não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo posição não pode ser vazio.");

            RuleFor(x => x.Status)
              .NotNull()
              .WithMessage("O campo posição não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo posição não pode ser vazio.")

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
                   }).WithMessage("Não é possivel inserir cadeiras com o mesmo id em um setor.");

                RuleForEach(x => x.Cadeiras)
                   .SetValidator(new ReferenciaCadeiraValidator(cadeiraApplication));
            });

            When(x => x.Mesas.Count > 0 && x.Mesas != null, () =>
            {
                RuleFor(dto => dto.Mesas)
                   .Must((dto, cancellation) =>
                   {
                       return ValidarInputListaMesa(dto);
                   }).WithMessage("Não é possivel inserir mesas com o mesmo id em um setor.");

                RuleForEach(x => x.Mesas)
                   .SetValidator(new ReferenciaMesaValidator(mesaApplication));
            });
        }

        private static bool ValidarInputListaCadeira(PostSetorDto postMesaDto)
        {
            return postMesaDto.Cadeiras.GroupBy(x => x.Id).All(g => g.Count() == 1);
        }

        private static bool ValidarInputListaMesa(PostSetorDto postMesaDto)
        {
            return postMesaDto.Mesas.GroupBy(x => x.Id).All(g => g.Count() == 1);
        }

        private bool ExisteLocalId(Guid localId)
        {
            return localApplication.ExisteLocalId(localId);
        }
    }
}