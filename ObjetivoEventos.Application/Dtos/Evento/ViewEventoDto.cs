using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Dtos.Evento
{
    /// <summary>
    /// Objeto utilizado para visualização.
    /// </summary>
    public class ViewEventoDto
    {
        public Guid Id { get; set; }

        public Guid LocalId { get; set; }

        public string Nome { get; set; }

        public string Sobre { get; set; }

        public DateTime DataEvento { get; set; }

        public int Duracao { get; set; }

        public string Cantor { get; set; }

        public string CaminhoAbsoluto { get; set; }

        public string CaminhoRelativo { get; set; }

        public Status Status { get; set; }
    }
}