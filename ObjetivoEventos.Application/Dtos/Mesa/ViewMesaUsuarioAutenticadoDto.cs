using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Dtos.Mesa
{
    /// <summary>
    /// Objeto utilizado para visualização de detalhes do pedido autenticado.
    /// </summary>
    public class ViewMesaUsuarioAutenticadoDto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public Fileira Fileira { get; set; }

        public int Coluna { get; set; }

        public Status Status { get; set; }
    }
}