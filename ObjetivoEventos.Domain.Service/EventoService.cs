using ObjetivoEventos.Domain.Core.Interfaces.Repositories;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using ObjetivoEventos.Domain.Service.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Service
{
    public class EventoService : ServiceBase<Evento>, IEventoService
    {
        private readonly IEventoRepository eventoRepository;

        public EventoService(IEventoRepository eventoRepository) : base(eventoRepository)
        {
            this.eventoRepository = eventoRepository;
        }

        public async Task<Evento> GetDisponibilidadeAsync(Guid eventoId, Guid setorId)
        {
            return await eventoRepository.GetDisponibilidadeAsync(eventoId, setorId);
        }

        public async Task<Evento> GetDetalhesAsync(Guid eventoId)
        {
            return await eventoRepository.GetDetalhesAsync(eventoId);
        }

        public async Task<PagedList<Evento>> GetPaginationAsync(ParametersEvento parametersEvento)
        {
            return await eventoRepository.GetPaginationAsync(parametersEvento);
        }

        public async Task<List<Evento>> GetEventosExpiradosAsync()
        {
            return await eventoRepository.GetEventosExpiradosAsync();
        }

        public async Task<List<Evento>> PutStatusRangeAsync(List<Guid> ids, Status status)
        {
            return await eventoRepository.PutStatusRangeAsync(ids, status);
        }

        public bool ValidarEventoExpirado(Guid id)
        {
            return eventoRepository.ValidarEventoExpirado(id);
        }

        public bool ValidarId(Guid id)
        {
            return eventoRepository.ValidarId(id);
        }

        public bool ValidarDataHora(Evento evento)
        {
            return eventoRepository.ValidarDataHora(evento);
        }
    }
}