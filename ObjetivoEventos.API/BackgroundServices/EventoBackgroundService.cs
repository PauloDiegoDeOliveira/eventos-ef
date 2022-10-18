using Microsoft.Extensions.DependencyInjection;
using ObjetivoEventos.Application.Dtos.Evento;
using ObjetivoEventos.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.BackgroundServices
{
    public class EventoBackgroundService
    {
        private readonly IServiceScope scope;
        private readonly IEventoApplication eventoApplication;

        public EventoBackgroundService(IServiceProvider serviceProvider)
        {
            scope = serviceProvider.CreateScope();
            eventoApplication = scope.ServiceProvider.GetRequiredService<IEventoApplication>();
        }

        public async Task<List<ViewEventoDto>> GetEventosExpiradosAsync()
        {
            return await eventoApplication.GetEventosExpiradosAsync();
        }

        public async Task<List<ViewEventoDto>> InativaEventosExpiradosAsync(List<ViewEventoDto> viewEventosDto)
        {
            List<Guid> ids = viewEventosDto.Select(x => x.Id).ToList();
            return await eventoApplication.PutStatusRangeAsync(ids, Domain.Enums.Status.Inativo);
        }
    }
}