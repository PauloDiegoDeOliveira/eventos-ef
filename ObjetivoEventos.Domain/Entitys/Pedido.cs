using ObjetivoEventos.Domain.Entitys.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ObjetivoEventos.Domain.Entitys
{
    public class Pedido : EntityBase
    {
        public Guid UsuarioId { get; private set; }

        public float ValorTotal { get; private set; }

        public string SituacaoPedido { get; private set; }

        public long Numero { get; private set; }

        public List<Reserva> Reservas { get; private set; }

        protected Pedido()
        { }

        public bool VerificaRegraSituacaoPedido(string situacaoPedido)
        {
            Dictionary<string, List<string>> regra = new()
                {
                    { "AguardandoPagamento", new List<string> { "PagamentoRealizado", "Cancelado" } },
                    { "PagamentoRealizado", new List<string> { "PagamentoAprovado", "Cancelado" } },
                    { "PagamentoAprovado", new List<string> { "Estorno", "Cancelado" } },
                    { "Estorno", new List<string> {  } },
                    { "Cancelado", new List<string> {  } }
                };

            if (!regra.ContainsKey(SituacaoPedido))
                return false;

            if (regra[SituacaoPedido].Contains(situacaoPedido))
                return true;

            return false;
        }

        public void AlterarSituacaoPedido(string situacaoPedido)
        {
            SituacaoPedido = situacaoPedido;
        }

        public void AlteraUsuarioId(Guid usuarioId)
        {
            UsuarioId = usuarioId;
        }

        public void AlterarReservas(List<Reserva> reservas)
        {
            Reservas = reservas;
        }

        public void AlterarValorTotal(float valorTotal)
        {
            ValorTotal = valorTotal;
        }

        public void AlterarNumero(long numero)
        {
            Numero = numero;
        }

        public IQueryable<Pedido> Where(Func<object, bool> value)
        {
            throw new NotImplementedException();
        }
    }
}