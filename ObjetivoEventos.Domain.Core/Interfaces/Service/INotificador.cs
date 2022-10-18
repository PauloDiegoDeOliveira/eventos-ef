using ObjetivoEventos.Domain.Core.Notificacoes;
using System.Collections.Generic;

namespace ObjetivoEventos.Domain.Core.Interfaces.Service
{
    public interface INotificador
    {
        bool TemNotificacao();

        List<Notificacao> ObterNotificacoes();

        void Handle(Notificacao notificacao);
    }
}