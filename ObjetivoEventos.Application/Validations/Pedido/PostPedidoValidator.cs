using FluentValidation;
using ObjetivoEventos.Application.Dtos.Pedido;
using ObjetivoEventos.Application.Interfaces;
using ObjetivoEventos.Application.Validations.Reserva;
using System.Linq;

namespace ObjetivoEventos.Application.Validations.Pedido
{
    public class PostPedidoValidator : AbstractValidator<PostPedidoDto>
    {
        private readonly IPedidoApplication pedidoApplication;
        private readonly IReservaApplication reservaApplication;

        public PostPedidoValidator(IReservaApplication reservaApplication, IPedidoApplication pedidoApplication)
        {
            this.pedidoApplication = pedidoApplication;
            this.reservaApplication = reservaApplication;

            RuleFor(x => x.Reservas)
              .NotNull()
              .WithMessage("O campo reservas não pode ser nulo.")

              .NotEmpty()
              .WithMessage("O campo reservas não pode ser vazio.");

            RuleFor(x => x.Status)
             .NotNull()
             .WithMessage("O campo status não pode ser nulo.")

             .NotEmpty()
             .WithMessage("O campo status não pode ser vazio.")

             .IsInEnum()
             .WithMessage("O valor do campo status não é valido.");

            When(x => x.Reservas.Count > 0, () =>
            {
                RuleFor(dto => dto)
                   .Must((dto, cancellation) =>
                   {
                       return ValidarInputLista(dto);
                   }).WithMessage("Não é possivel inserir reservas com o mesmo id no pedido.");

                RuleForEach(x => x.Reservas).SetValidator(new ReferenciaReservaValidator(reservaApplication));

                RuleFor(dto => dto)
                   .Must((dto, cancellation) =>
                   {
                       return !ValidarReservaPedidos(dto);
                   }).WithMessage("Já existe uma reserva cadastrada em outro pedido.");

                RuleFor(dto => dto)
                   .Must((dto, cancellation) =>
                   {
                       return ValidarInputReservaEventoPedidos(dto);
                   }).WithMessage("As reservas cadastradas não são do mesmo evento.");
            });
        }

        private bool ValidarInputLista(PostPedidoDto postPedidoDto)
        {
            return postPedidoDto.Reservas.GroupBy(x => x.Id).All(g => g.Count() == 1);
        }

        private bool ValidarInputReservaEventoPedidos(PostPedidoDto postPedidoDto)
        {
            return pedidoApplication.ValidarInputReservaEventoPedidos(postPedidoDto);
        }

        private bool ValidarReservaPedidos(PostPedidoDto postPedidoDto)
        {
            return pedidoApplication.ValidarReservaPedidos(postPedidoDto);
        }
    }
}