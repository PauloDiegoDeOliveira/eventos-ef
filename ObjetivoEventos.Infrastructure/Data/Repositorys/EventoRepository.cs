using Microsoft.EntityFrameworkCore;
using ObjetivoEventos.Domain.Core.Interfaces.Repositories;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using ObjetivoEventos.Infrastructure.Data.Repositorys.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjetivoEventos.Infrastructure.Data.Repositorys
{
    public class EventoRepository : RepositoryBase<Evento>, IEventoRepository
    {
        private readonly AppDbContext appDbContext;

        public EventoRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<PagedList<Evento>> GetPaginationAsync(ParametersEvento parametersEvento)
        {
            IQueryable<Evento> eventos = appDbContext.Eventos.OrderByDescending(evento => evento.CriadoEm);

            if (parametersEvento.PalavraChave == null && parametersEvento.Id == null && parametersEvento.Status == 0)
                eventos = eventos.Where(x => x.Status != Status.Excluido.ToString());
            else if (parametersEvento.Status != 0)
                eventos = eventos.Where(x => x.Status == parametersEvento.Status.ToString());

            if (!string.IsNullOrEmpty(parametersEvento.PalavraChave))
                eventos = eventos.Where(x => EF.Functions.Like(x.Nome, $"%{parametersEvento.PalavraChave}%"));

            if (parametersEvento.Id != null)
                eventos = eventos.Where(x => parametersEvento.Id.Contains(x.Id));

            if (parametersEvento.Ordenar != 0)
            {
                switch (parametersEvento.Ordenar.ToString())
                {
                    case "Crescente":
                        eventos = eventos.OrderBy(x => x.Nome);
                        break;

                    case "Decrescente":
                        eventos = eventos.OrderByDescending(x => x.Nome);
                        break;

                    case "Novos":
                        eventos = eventos.OrderByDescending(x => x.CriadoEm);
                        break;

                    case "Antigos":
                        eventos = eventos.OrderBy(x => x.CriadoEm);
                        break;
                }
            }

            return await Task.FromResult(PagedList<Evento>.ToPagedList(eventos.AsNoTracking(), parametersEvento.NumeroPagina, parametersEvento.ResultadosExibidos));
        }

        public async Task<Evento> GetDisponibilidadeAsync(Guid eventoId, Guid setorId)
        {
            IQueryable<Evento> evento = appDbContext.Set<Evento>();

            evento = evento.Include(x => x.Local)
                .ThenInclude(x => x.Setores.Where(x => x.Id == setorId))
                    .ThenInclude(x => x.Mesas)
                         .ThenInclude(x => x.Cadeiras.OrderBy(cadeira => cadeira.Fileira).ThenBy(cadeira => cadeira.Coluna))
                            .Include(x => x.Local)
                                .ThenInclude(x => x.Setores)
                                    .ThenInclude(x => x.Cadeiras.OrderBy(cadeira => cadeira.Fileira).ThenBy(cadeira => cadeira.Coluna))
                                        .Include(x => x.Reservas.Where(x => x.SetorId == setorId && x.SituacaoReserva != SituacaoReserva.Cancelada.ToString() && x.SituacaoReserva != SituacaoReserva.PagamentoNaoFinalizado.ToString() && x.Status == Status.Ativo.ToString()))
                                            .AsNoTracking();

            return await evento.FirstOrDefaultAsync(x => x.Id == eventoId);
        }

        public async Task<Evento> GetDetalhesAsync(Guid eventoId)
        {
            return await appDbContext.Set<Evento>()
                .Include(x => x.Local)
                .ThenInclude(x => x.Setores.OrderBy(setor => setor.Nome))
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == eventoId);
        }

        public async Task<List<Evento>> GetReservasByListId(List<Guid> ids)
        {
            return await appDbContext.Eventos
                 .Where(evento => ids.Contains(evento.Id))
                 .ToListAsync();
        }

        //TODO: Verificar se vai dar problema se não validar apenas pelo dia e por Ativo
        public async Task<List<Evento>> GetEventosExpiradosAsync()
        {
            return await appDbContext.Eventos.Where(x => x.DataEvento.AddHours(x.Duracao) < DateTime.Now && x.Status == Status.Ativo.ToString()).ToListAsync();
        }

        public async Task<List<Evento>> PutStatusRangeAsync(List<Guid> ids, Status status)
        {
            List<Evento> eventos = await GetReservasByListId(ids);

            foreach (Evento evento in eventos)
            {
                evento.ChangeStatusValue(status.ToString());
            }

            appDbContext.UpdateRange(eventos);
            await appDbContext.SaveChangesAsync();
            return eventos;
        }

        public bool ValidarId(Guid id)
        {
            return appDbContext.Eventos.Any(x => x.Id == id);
        }

        public bool ValidarEventoExpirado(Guid id)
        {
            Evento evento = appDbContext.Eventos.AsNoTracking().FirstOrDefault(x => x.Id == id);

            if (evento == null)
                return false;

            return DateTime.Today > evento.DataEvento && DateTime.Now > evento.DataEvento.AddHours(evento.Duracao);
        }

        public bool ValidarDataHora(Evento evento)
        {
            if (evento.Id == Guid.Empty)
                return appDbContext.Eventos
                              .AsNoTracking()
                              .Any(x => x.DataEvento == evento.DataEvento && x.LocalId == evento.LocalId && x.Status != Status.Excluido.ToString());
            else
                return appDbContext.Eventos
                               .AsNoTracking()
                               .Any(x => x.DataEvento == evento.DataEvento && x.LocalId == evento.LocalId && x.Id != evento.Id && x.Status != Status.Excluido.ToString());
        }
    }
}