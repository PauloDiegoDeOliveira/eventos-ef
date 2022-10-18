using ObjetivoEventos.Domain.Entitys.Base;
using System;
using System.Collections.Generic;

namespace ObjetivoEventos.Domain.Entitys
{
    public class Evento : UploadIFormFileBase
    {
        public Guid LocalId { get; private set; }

        public string Nome { get; private set; }

        public string Sobre { get; private set; }

        public DateTime DataEvento { get; private set; }

        public int Duracao { get; private set; }

        public string Cantor { get; private set; }

        public Local Local { get; private set; }

        public List<Reserva> Reservas { get; private set; }

        public Evento()
        { }

        public void PopulaLocal(Local local)
        {
            Local = local;
        }
    }
}