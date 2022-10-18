using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Application.Dtos.Mesa;
using ObjetivoEventos.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ObjetivoEventos.Application.Dtos.Setor
{
    /// <summary>
    /// Objeto utilizado para visualização.
    /// </summary>
    public class ViewSetorDto
    {
        public Guid Id { get; set; }

        public Guid LocalId { get; set; }

        public string Nome { get; set; }

        public float Preco { get; set; }

        public string Posicao { get; set; }

        public string Alias { get; set; }

        public Status Status { get; set; }

        public List<ViewMesaDto> Mesas { get; set; }

        public List<ViewCadeiraDto> Cadeiras { get; set; }
    }
}