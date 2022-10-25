using ObjetivoEventos.Application.Dtos.Local;
using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Dtos.Evento
{
    /// <summary>
    /// Objeto utilizado para visualização de detalhes do pedido autenticado.
    /// </summary>
    public class ViewEventoUsuarioAutenticadoDto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Sobre { get; set; }

        public DateTime DataEvento { get; set; }

        public int Duracao { get; set; }

        public string CaminhoRelativo { get; set; }

        public Status Status { get; set; }

        public ViewLocalDto Local { get; set; }
    }
}