using ObjetivoEventos.Domain.Entitys.Base;
using System.Collections.Generic;

namespace ObjetivoEventos.Domain.Entitys
{
    public class Cadeira : EntityBase
    {
        public string Nome { get; private set; }

        public string Fileira { get; private set; }

        public int Coluna { get; private set; }

        public List<Setor> Setores { get; private set; }

        public List<Mesa> Mesas { get; private set; }

        protected Cadeira()
        { }
    }
}