using ObjetivoEventos.Domain.Entitys.Base;
using System;
using System.Collections.Generic;

namespace ObjetivoEventos.Domain.Entitys
{
    public class Reserva : EntityBase
    {
        public Guid EventoId { get; private set; }

        public Guid LocalId { get; private set; }

        public Guid SetorId { get; private set; }

        public Guid? MesaId { get; private set; }

        public Guid CadeiraId { get; private set; }

        public Guid UsuarioId { get; private set; }

        public string ConnectionId { get; private set; }

        public string SituacaoReserva { get; private set; }

        public Evento Evento { get; private set; }

        public List<Pedido> Pedidos { get; private set; }

        protected Reserva()
        { }

        public bool VerificaRegraSituacaoReserva(string situacaoReserva)
        {
            Dictionary<string, List<string>> regra = new()
                {
                    { "Reservado", new List<string> { "AguardandoPagamento", "Cancelada" } },
                    { "AguardandoPagamento", new List<string> { "CompraFinalizada", "PagamentoNaoFinalizado", "Cancelada" } },
                    { "CompraFinalizada", new List<string> { "Utilizada", "ValorEstornado", "Cancelada" } },
                    { "PagamentoNaoFinalizado", new List<string> { } },
                    { "Utilizada", new List<string> { } },
                    { "Cancelada", new List<string> { "Reservado" } }
            };

            if (!regra.ContainsKey(SituacaoReserva))
                return false;

            if (regra[SituacaoReserva].Contains(situacaoReserva))
                return true;

            return false;
        }

        public void AlterarSituacaoReserva(string situacaoReserva)
        {
            SituacaoReserva = situacaoReserva;
        }

        public void AlterarUsuarioId(Guid usuarioId)
        {
            UsuarioId = usuarioId;
        }
    }
}