using ObjetivoEventos.Domain.Entitys.Base;
using System;
using System.Collections.Generic;

namespace ObjetivoEventos.Domain.Entitys
{
    public class Setor : EntityBase
    {
        public Guid LocalId { get; private set; }

        public string Nome { get; private set; }

        public float Preco { get; private set; }

        public string Posicao { get; private set; }

        public string Alias { get; private set; }

        public Local Local { get; private set; }

        public List<Mesa> Mesas { get; private set; }

        public List<Cadeira> Cadeiras { get; private set; }

        protected Setor()
        { }

        public void ListaMesas(List<Mesa> mesas)
        {
            Mesas = mesas;
        }

        public void ListaCadeiras(List<Cadeira> cadeiras)
        {
            Cadeiras = cadeiras;
        }
    }
}