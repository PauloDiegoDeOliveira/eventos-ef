using ObjetivoEventos.Domain.Entitys.Base;
using System.Collections.Generic;

namespace ObjetivoEventos.Domain.Entitys
{
    public class Local : EntityBase
    {
        public string Nome { get; private set; }

        public string Telefone { get; private set; }

        public string CEP { get; private set; }

        public List<Evento> Eventos { get; private set; }

        public List<Setor> Setores { get; private set; }
    }
}