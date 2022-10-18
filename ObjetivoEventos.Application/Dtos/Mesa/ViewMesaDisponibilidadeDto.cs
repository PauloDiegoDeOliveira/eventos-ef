using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ObjetivoEventos.Application.Dtos.Mesa
{
    /// <summary>
    /// Objeto utilizado para visualização de disponibilidade.
    /// </summary>
    public class ViewMesaDisponibilidadeDto
    {
        public Guid Id { get; private set; }

        public string Nome { get; private set; }

        public string Alias { get; private set; }

        public Fileira Fileira { get; private set; }

        public int Coluna { get; private set; }

        public Status Status { get; private set; }

        public StatusDisponibilidade StatusDisponibilidade { get; private set; } = StatusDisponibilidade.Disponivel;

        public List<ViewCadeiraDisponibilidadeDto> Cadeiras { get; private set; }

        public ViewMesaDisponibilidadeDto(Domain.Entitys.Mesa mesa)
        {
            Id = mesa.Id;
            Nome = mesa.Nome;
            Alias = mesa.Alias;
            Coluna = mesa.Coluna;
            Cadeiras = PopulaCadeiras(mesa.Cadeiras);
            _ = Enum.TryParse(mesa.Fileira, out Fileira myFileira);
            Fileira = myFileira;
            _ = Enum.TryParse(mesa.Status, out Status myStatus);
            Status = myStatus;
        }

        private List<ViewCadeiraDisponibilidadeDto> PopulaCadeiras(List<Domain.Entitys.Cadeira> listaCadeiras)
        {
            List<ViewCadeiraDisponibilidadeDto> list = new();

            foreach (Domain.Entitys.Cadeira cadeira in listaCadeiras)
                list.Add(new ViewCadeiraDisponibilidadeDto(cadeira));

            return list;
        }

        public bool EstaDisponivel()
        {
            bool estaDisponivel = Cadeiras.Exists(x => x.StatusDisponibilidade == StatusDisponibilidade.Disponivel);

            if (!estaDisponivel)
                StatusDisponibilidade = StatusDisponibilidade.Indisponivel;

            return estaDisponivel;
        }
    }
}