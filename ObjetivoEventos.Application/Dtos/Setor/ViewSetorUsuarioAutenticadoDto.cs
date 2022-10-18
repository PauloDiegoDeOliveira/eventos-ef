using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Dtos.Setor
{
    /// <summary>
    /// Objeto utilizado para visualização de detalhes do pedido autenticado.
    /// </summary>
    public class ViewSetorUsuarioAutenticadoDto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public float Preco { get; set; }

        public Status Status { get; set; }
    }
}