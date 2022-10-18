//using FluentValidation;
//using ObjetivoEventos.Application.Interfaces;
//using ObjetivoEventos.Application.Validations.Documentos;
//using System;

//namespace ObjetivoEventos.Application.Validations.Usuario
//{
//    public class PutUsuarioValidator : AbstractValidator<PutUsuarioDto>
//    {
//        private readonly IUsuarioApplication usuarioApplication;

//        public PutUsuarioValidator(IUsuarioApplication usuarioApplication)
//        {
//            this.usuarioApplication = usuarioApplication;

//            RuleFor(x => x.Id)
//               .NotNull()
//               .WithMessage("O id do usuário não pode ser nulo.")

//               .NotEmpty()
//               .WithMessage("O id do usuário não pode ser vazio.")

//               .Must((dto, cancelar) =>
//               {
//                   return ValidarId(dto.Id);
//               }).WithMessage("Nenhum usuário foi encontrado com o id informado.");

//            RuleFor(x => x.Nome)
//              .NotNull()
//              .WithMessage("O nome não pode ser nulo.")

//              .NotEmpty()
//              .WithMessage("O nome não pode ser vazio.");

//            RuleFor(x => x.Sobrenome)
//              .NotNull()
//              .WithMessage("O sobrenome não pode ser nulo.")

//              .NotEmpty()
//              .WithMessage("O sobrenome não pode ser vazio.");

//            RuleFor(x => x.RA)
//              .NotNull()
//              .WithMessage("O RA não pode ser nulo.")

//              .NotEmpty()
//              .WithMessage("O RA não pode ser vazio.");

//            RuleFor(x => x.Email)
//              .NotNull()
//              .WithMessage("O e-mail não pode ser nulo.")

//              .NotEmpty()
//              .WithMessage("O e-mail não pode ser vazio.")

//              .EmailAddress()
//              .WithMessage("Um endereço de e-mail válido é necessário.");

//            RuleFor(x => x.Senha)
//              .NotNull()
//              .WithMessage("A senha não pode ser nulo.")

//              .NotEmpty()
//              .WithMessage("A senha não pode ser vazio.");

//            RuleFor(x => x.ConfirmarSenha)
//              .NotNull()
//              .WithMessage("Confirmar senha não pode ser nulo.")

//              .NotEmpty()
//              .WithMessage("Confirmar senha não pode ser vazio.");

//            RuleFor(x => x.CPF.Length)
//              .Equal(CpfValidacao.TamanhoCpf)
//              .WithMessage("O campo CPF precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");

//            RuleFor(x => CpfValidacao.Validar(x.CPF))
//                .Equal(true)
//                .WithMessage("O CPF fornecido é inválido.");

//            RuleFor(x => x.CPF)
//              .NotNull()
//              .WithMessage("O CPF não pode ser nulo.")

//              .NotEmpty()
//              .WithMessage("O CPF não pode ser vazio.");

//            RuleFor(x => x.Nascimento)
//              .NotNull()
//              .WithMessage("CPF não pode ser nulo.")

//              .NotEmpty()
//              .WithMessage("CPF não pode ser vazio.");

//            RuleFor(x => x.Genero)
//              .NotNull()
//              .WithMessage("O gênero não pode ser nulo.")

//              .NotEmpty()
//              .WithMessage("O gênero não pode ser vazio.");

//            RuleFor(x => x.Status)
//             .NotNull()
//             .WithMessage("O status não pode ser nulo.")

//             .NotEmpty()
//             .WithMessage("O status não pode ser vazio.");
//        }

//        private bool ValidarId(Guid id)
//        {
//            return usuarioApplication.ValidarId(id);
//        }
//    }
//}