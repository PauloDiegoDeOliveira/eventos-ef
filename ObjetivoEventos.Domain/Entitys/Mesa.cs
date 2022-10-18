using ObjetivoEventos.Domain.Entitys.Base;
using System.Collections.Generic;

namespace ObjetivoEventos.Domain.Entitys
{
    public class Mesa : EntityBase
    {
        public string Nome { get; private set; }

        public string Alias { get; private set; }

        public string Fileira { get; private set; }

        public int Coluna { get; private set; }

        public List<Setor> Setores { get; private set; }

        public List<Cadeira> Cadeiras { get; private set; }

        protected Mesa()
        { }

        public void ListaCadeiras(List<Cadeira> cadeiras)
        {
            Cadeiras = cadeiras;
        }
    }
}