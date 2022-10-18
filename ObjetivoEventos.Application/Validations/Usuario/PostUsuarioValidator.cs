using FluentValidation;
using ObjetivoEventos.Application.Dtos.Identity.Usuario;
using ObjetivoEventos.Application.Validations.Documentos;

namespace ObjetivoEventos.Application.Validations.Usuario
{
    public class PostUsuarioValidator : AbstractValidator<PostCadastroUsuarioDto>
    {
        public PostUsuarioValidator()
        {
            RuleFor(x => x.Nome)
              .NotNull()
              .WithMessage("O campo nome não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo nome não pode ser vazio.");

            RuleFor(x => x.Sobrenome)
              .NotNull()
              .WithMessage("O campo sobrenome não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo sobrenome não pode ser vazio.");

            RuleFor(x => x.RA)
              .NotNull()
              .WithMessage("O campo RA não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo RA não pode ser vazio.");

            RuleFor(x => x.Email)
              .NotNull()
              .WithMessage("O campo e-mail não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo e-mail não pode ser vazio.")

              .EmailAddress()
              .WithMessage("Um endereço de e-mail válido é necessário.");

            RuleFor(x => x.Senha)
              .NotNull()
              .WithMessage("O campo senha não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo senha não pode ser vazio.");

            RuleFor(x => x.ConfirmarSenha)
              .NotNull()
              .WithMessage("O campo confirmar senha não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo confirmar senha não pode ser vazio.");

            RuleFor(x => x.CPF.Length)
              .Equal(CpfValidacao.TamanhoCpf)
              .WithMessage("O campo CPF precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");

            RuleFor(x => CpfValidacao.Validar(x.CPF))
                .Equal(true)
                .WithMessage("O CPF fornecido é inválido.");

            RuleFor(x => x.CPF)
              .NotNull()
              .WithMessage("O campo CPF não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo CPF não pode ser vazio.");

            RuleFor(x => x.Nascimento)
              .NotNull()
              .WithMessage("O campo data de nascimento não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo data de nascimento não pode ser vazio.");

            RuleFor(x => x.Genero)
              .NotNull()
              .WithMessage("O campo gênero não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo gênero não pode ser vazio.")

              .IsInEnum()
              .WithMessage("O valor do campo gênero não é valido.");

            RuleFor(x => x.Status)
              .NotNull()
              .WithMessage("O campo status não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo status não pode ser vazio.")

               .IsInEnum()
              .WithMessage("O valor do campo status não é valido.");
        }
    }
}