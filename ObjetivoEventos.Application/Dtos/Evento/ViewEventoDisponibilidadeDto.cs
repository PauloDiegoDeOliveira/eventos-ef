using ObjetivoEventos.Application.Dtos.Local;
using ObjetivoEventos.Application.Dtos.Reserva;
using System;
using System.Collections.Generic;

namespace ObjetivoEventos.Application.Dtos.Evento
{
    /// <summary>
    /// Objeto utilizado para visualização de disponibilidade.
    /// </summary>
    public class ViewEventoDisponibilidadeDto
    {
        public Guid Id { get; private set; }

        public ViewLocalDisponibilidadeDto Local { get; private set; }

        public List<ViewReservaDto> Reservas { get; private set; }

        public ViewEventoDisponibilidadeDto(Domain.Entitys.Evento evento,
                                         ViewLocalDisponibilidadeDto local,
                                         List<ViewReservaDto> viewReservaDtos)
        {
            Id = evento.Id;
            Local = local;
            Reservas = viewReservaDtos;
        }
    }
}