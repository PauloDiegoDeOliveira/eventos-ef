using ObjetivoEventos.Domain.Enums;
using System;

namespace ObjetivoEventos.Application.Dtos.Cadeira
{
    /// <summary>
    /// Objeto utilizado para visualização de disponibilidade.
    /// </summary>
    public class ViewCadeiraDisponibilidadeDto
    {
        public Guid Id { get; private set; }

        public string Nome { get; private set; }

        public Fileira Fileira { get; private set; }

        public int Coluna { get; private set; }

        public Status Status { get; private set; }

        public StatusDisponibilidade StatusDisponibilidade { get; private set; } = StatusDisponibilidade.Disponivel;

        public ViewCadeiraDisponibilidadeDto(Domain.Entitys.Cadeira cadeira)
        {
            Id = cadeira.Id;
            Nome = cadeira.Nome;
            Coluna = cadeira.Coluna;
            _ = Enum.TryParse(cadeira.Fileira, out Fileira myFileira);
            Fileira = myFileira;
            _ = Enum.TryParse(cadeira.Status, out Status myStatus);
            Status = myStatus;
        }

        public bool EstaDisponivel()
        {
            if (StatusDisponibilidade == StatusDisponibilidade.Disponivel)
                return true;
            else
                return false;
        }

        public void AlterarDisponibilidade(StatusDisponibilidade statusDisponibilidade)
        {
            StatusDisponibilidade = statusDisponibilidade;
        }
    }
}