using ObjetivoEventos.Application.Dtos.Setor;
using ObjetivoEventos.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ObjetivoEventos.Application.Dtos.Local
{
    /// <summary>
    /// Objeto utilizado para visualização de detalhes.
    /// </summary>
    public class ViewLocalDetalhesDto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Telefone { get; set; }

        public string CEP { get; set; }

        public Status Status { get; set; }

        public List<ViewSetorDto> Setores { get; set; }
    }
}