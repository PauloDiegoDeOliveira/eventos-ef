using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Dtos.Cadeira
{
    /// <summary>
    /// Objeto utilizado para visualização.
    /// </summary>
    public class ViewCadeiraDto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public Fileira Fileira { get; set; }

        public int Coluna { get; set; }

        public Status Status { get; set; }
    }
}