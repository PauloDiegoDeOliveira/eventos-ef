using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ObjetivoEventos.Application.Dtos.Mesa
{
    /// <summary>
    /// Objeto utilizado para visualização.
    /// </summary>
    public class ViewMesaDto
    {
        public Guid Id { get; set; }

        public string Nome { get; set; }

        public string Alias { get; set; }

        public Fileira Fileira { get; set; }

        public int Coluna { get; set; }

        public Status Status { get; set; }

        public List<ViewCadeiraDto> Cadeiras { get; set; }
    }
}