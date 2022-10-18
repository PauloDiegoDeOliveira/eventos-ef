using System;

namespace ObjetivoEventos.Domain.Entitys.Base
{
    public abstract class EntityBase
    {
        public Guid Id { get; private set; }

        public string Status { get; private set; }

        public DateTime CriadoEm { get; private set; } = DateTime.Now;

        public DateTime? AlteradoEm { get; private set; }

        protected EntityBase()
        { }

        public void ChangeStatusValue(string status)
        {
            Status = status;
        }

        public void GuidValue(Guid id)
        {
            Id = id;
        }
    }
}