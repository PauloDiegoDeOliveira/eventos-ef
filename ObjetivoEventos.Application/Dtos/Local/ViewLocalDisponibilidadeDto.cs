using ObjetivoEventos.Application.Dtos.Setor;
using System;

namespace ObjetivoEventos.Application.Dtos.Local
{
    /// <summary>
    /// Objeto utilizado para visualização de disponibilidade.
    /// </summary>
    public class ViewLocalDisponibilidadeDto
    {
        public Guid Id { get; private set; }

        public ViewSetorDisponibilidadeDto Setor { get; private set; }

        public ViewLocalDisponibilidadeDto(Domain.Entitys.Local local, ViewSetorDisponibilidadeDto setor)
        {
            Id = local.Id;
            Setor = setor;
        }
    }
}