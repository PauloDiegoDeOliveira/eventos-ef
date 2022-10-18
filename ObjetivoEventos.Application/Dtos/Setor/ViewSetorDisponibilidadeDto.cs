using ObjetivoEventos.Application.Dtos.Cadeira;
using ObjetivoEventos.Application.Dtos.Mesa;
using ObjetivoEventos.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ObjetivoEventos.Application.Dtos.Setor
{
    /// <summary>
    /// Objeto utilizado para visualização da disponibilidade.
    /// </summary>
    public class ViewSetorDisponibilidadeDto
    {
        public Guid Id { get; private set; }

        public StatusDisponibilidade StatusDisponibilidade { get; private set; } = StatusDisponibilidade.Disponivel;

        public List<ViewMesaDisponibilidadeDto> Mesas { get; private set; }

        public List<ViewCadeiraDisponibilidadeDto> Cadeiras { get; private set; }

        public ViewSetorDisponibilidadeDto(Domain.Entitys.Setor setor)
        {
            Id = setor.Id;
            Cadeiras = PopulaCadeiras(setor.Cadeiras);
            Mesas = PopulaMesas(setor.Mesas);
        }

        private List<ViewCadeiraDisponibilidadeDto> PopulaCadeiras(List<Domain.Entitys.Cadeira> listaCadeiras)
        {
            List<ViewCadeiraDisponibilidadeDto> list = new();

            foreach (Domain.Entitys.Cadeira cadeira in listaCadeiras)
                list.Add(new ViewCadeiraDisponibilidadeDto(cadeira));

            return list;
        }

        private List<ViewMesaDisponibilidadeDto> PopulaMesas(List<Domain.Entitys.Mesa> listaMesas)
        {
            List<ViewMesaDisponibilidadeDto> list = new();

            foreach (Domain.Entitys.Mesa mesa in listaMesas)
                list.Add(new ViewMesaDisponibilidadeDto(mesa));

            return list;
        }

        public bool EstaDisponivel()
        {
            if (MesasDisponiveis() || CadeirasDisponiveis())
            {
                StatusDisponibilidade = StatusDisponibilidade.Disponivel;
                return true;
            }
            else
            {
                StatusDisponibilidade = StatusDisponibilidade.Indisponivel;
                return false;
            }
        }

        public bool CadeirasDisponiveis()
        {
            return Cadeiras.Exists(x => x.StatusDisponibilidade == StatusDisponibilidade.Disponivel);
        }

        public bool MesasDisponiveis()
        {
            foreach (ViewMesaDisponibilidadeDto mesa in Mesas)
                mesa.EstaDisponivel();

            return Mesas.Exists(x => x.StatusDisponibilidade == StatusDisponibilidade.Disponivel);
        }
    }
}