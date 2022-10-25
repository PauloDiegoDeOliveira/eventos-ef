using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Dtos.Evento;
using ObjetivoEventos.Application.Dtos.Pagination;
using ObjetivoEventos.Application.Interfaces.Base;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Interfaces
{
    public interface IEventoApplication : IApplicationBase<Evento, ViewEventoDto, PostEventoDto, PutEventoDto, PutStatusDto>
    {
        Task<ViewPagedListDto<Evento, ViewEventoDto>> GetPaginationAsync(ParametersEvento parametersEvento);

        Task<ViewEventoDisponibilidadeDto> GetDisponibilidadeAsync(Guid eventoId, Guid setorId);

        Task<ViewEventoDetalhesDto> GetDetalhesAsync(Guid eventoId);

        Task<List<ViewEventoDto>> GetEventosExpiradosAsync();

        Task<ViewEventoDto> PostAsync(PostEventoDto postEventoDto, string caminhoFisico, string caminhoAbsoluto, string splitRelativo);

        Task<ViewEventoDto> PutAsync(PutEventoDto putEventoDto, string caminhoFisico, string caminhoAbsoluto, string splitRelativo);

        Task<List<ViewEventoDto>> PutStatusRangeAsync(List<Guid> ids, Status status);

        bool ValidarEventoExpirado(Guid id);

        bool ValidarId(Guid id);

        bool ValidarDataHoraPost(PostEventoDto postEventoDto);

        bool ValidarDataHoraPut(PutEventoDto putEventoDto);
    }
}