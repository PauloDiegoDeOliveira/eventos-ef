using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Dtos.Local
{
    /// <summary>
    /// Objeto utilizado para visualização.
    /// </summary>
    public class ViewLocalDto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Telefone { get; set; }

        public string CEP { get; set; }

        public Status Status { get; set; }
    }
}