using Microsoft.EntityFrameworkCore;
using ObjetivoEventos.Domain.Core.Interfaces.Repositories;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
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
    public class ReservaRepository : RepositoryBase<Reserva>, IReservaRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly IUser user;

        public ReservaRepository(AppDbContext appDbContext,
                                IUser user) : base(appDbContext)
        {
            this.appDbContext = appDbContext;
            this.user = user;
        }

        public async Task<PagedList<Reserva>> GetPaginationAsync(ParametersBase parametersBase)
        {
            IQueryable<Reserva> reservas = appDbContext.Set<Reserva>();

            if (parametersBase.Id == null && parametersBase.Status == 0)
                reservas = reservas.Where(x => x.Status != Status.Excluido.ToString());
            else if (parametersBase.Status != 0)
                reservas = reservas.Where(x => x.Status == parametersBase.Status.ToString());

            if (parametersBase.Id != null)
                reservas = reservas.Where(x => parametersBase.Id.Contains(x.Id));

            reservas = reservas.OrderBy(x => x.CriadoEm).AsNoTracking();

            return await Task.FromResult(PagedList<Reserva>.ToPagedList(reservas, parametersBase.NumeroPagina, parametersBase.ResultadosExibidos));
        }

        public async Task<Reserva> GetReservasDetalhesById(Guid id)
        {
            return await appDbContext.Reservas.AsNoTracking()
                .Include(x => x.Pedidos)
                .Include(x => x.Evento)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Reserva>> GetReservasByListId(List<Guid> ids)
        {
            return await appDbContext.Reservas
                 .Where(reserva => ids.Contains(reserva.Id))
                 .ToListAsync();
        }

        public async Task<List<Reserva>> GetReservasByListIdNoTracking(List<Guid> ids)
        {
            return await appDbContext.Reservas.AsNoTracking()
                         .Where(reserva => ids.Contains(reserva.Id))
                         .ToListAsync();
        }

        public async Task<List<Reserva>> GetReservasByTempoSituacaoAsync(int minutos, SituacaoReserva situacaoReserva)
        {
            return await appDbContext.Reservas.Where(x => x.SituacaoReserva == situacaoReserva.ToString() && x.CriadoEm.AddMinutes(minutos) < DateTime.Now && x.Status == Status.Ativo.ToString()).AsNoTracking().ToListAsync();
        }

        public override async Task<Reserva> PostAsync(Reserva reserva)
        {
            reserva.AlterarUsuarioId(user.GetUserId());
            return await base.PostAsync(reserva);
        }

        public override async Task<Reserva> PutAsync(Reserva reserva)
        {
            Reserva consulta = await GetByIdAsync(reserva.Id);
            reserva.AlterarSituacaoReserva(consulta.SituacaoReserva);
            reserva.AlterarUsuarioId(user.GetUserId());
            return await base.PutAsync(reserva);
        }

        public async Task<List<Reserva>> PutRangeAsync(List<Reserva> reservas)
        {
            appDbContext.UpdateRange(reservas);
            await appDbContext.SaveChangesAsync();
            return reservas;
        }

        public async Task<List<Reserva>> PutStatusRangeAsync(List<Guid> ids, Status status)
        {
            List<Reserva> reservas = await GetReservasByListId(ids);

            foreach (Reserva reserva in reservas)
            {
                reserva.ChangeStatusValue(status.ToString());
            }

            appDbContext.UpdateRange(reservas);
            await appDbContext.SaveChangesAsync();
            return reservas;
        }

        public async Task<List<Reserva>> DeleteRangeAsync(List<Reserva> reservas)
        {
            appDbContext.RemoveRange(reservas);
            await appDbContext.SaveChangesAsync();
            return reservas;
        }

        public async Task<List<Reserva>> DeleteByConnectionId(string connectionId)
        {
            List<Reserva> consulta = await appDbContext.Set<Reserva>().Where(x => x.ConnectionId == connectionId && x.SituacaoReserva == SituacaoReserva.Reservado.ToString()).ToListAsync();

            if (consulta is not null)
            {
                appDbContext.RemoveRange(consulta);
                await appDbContext.SaveChangesAsync();
            }

            return consulta;
        }

        public bool VerificaDisponibilidadeReserva(Reserva reserva)
        {
            if (reserva.Id != Guid.Empty)
            {
                if (reserva.MesaId == Guid.Empty || reserva.MesaId is null)
                    return appDbContext.Reservas.AsNoTracking().Any(x => x.Id != reserva.Id && x.CadeiraId == reserva.CadeiraId && x.SetorId == reserva.SetorId && x.EventoId == reserva.EventoId && x.Status == Status.Ativo.ToString());
                else
                    return appDbContext.Reservas.AsNoTracking().Any(x => x.Id != reserva.Id && x.MesaId == reserva.MesaId && x.CadeiraId == reserva.CadeiraId && x.SetorId == reserva.SetorId && x.EventoId == reserva.EventoId && x.Status == Status.Ativo.ToString());
            }
            else
            {
                if (reserva.MesaId == Guid.Empty || reserva.MesaId is null)
                    return appDbContext.Reservas.AsNoTracking().Any(x => x.CadeiraId == reserva.CadeiraId && x.SetorId == reserva.SetorId && x.EventoId == reserva.EventoId && x.Status == Status.Ativo.ToString());
                else
                    return appDbContext.Reservas.AsNoTracking().Any(x => x.MesaId == reserva.MesaId && x.CadeiraId == reserva.CadeiraId && x.SetorId == reserva.SetorId && x.EventoId == reserva.EventoId && x.Status == Status.Ativo.ToString());
            }
        }

        public bool ValidaListId(List<Guid> ids)
        {
            int reservas = appDbContext.Reservas.AsNoTracking().Where(x => ids.Contains(x.Id)).ToList().Count;

            if (reservas != ids.Count)
                return false;

            return true;
        }

        public bool ValidarSetorCadeiraMesa(Reserva reserva)
        {
            Setor setor = appDbContext.Setores.AsNoTracking()
                                        .Include(x => x.Mesas)
                                        .Include(x => x.Cadeiras)
                                        .FirstOrDefault(x => x.Id == reserva.SetorId);

            if (setor is null)
                return false;

            if (reserva.MesaId is not null)
            {
                Mesa mesa = setor.Mesas.Find(x => x.Id == reserva.MesaId);

                if (mesa is null)
                    return false;
                else if (mesa.Cadeiras.Any(x => x.Id == reserva.CadeiraId))
                    return true;
                else
                    return false;
            }
            else
            {
                return setor.Cadeiras.Any(x => x.Id == reserva.CadeiraId);
            }
        }

        public bool ValidaQuantidadeCadeiras(Reserva reserva)
        {
            int count = appDbContext.Reservas
            .AsNoTracking()
            .Where(x => x.UsuarioId == user.GetUserId() && x.EventoId == reserva.EventoId && x.Status == Status.Ativo.ToString() &&
            x.SituacaoReserva != SituacaoReserva.Cancelada.ToString() && x.SituacaoReserva != SituacaoReserva.PagamentoNaoFinalizado.ToString())
            .ToList().Count;

            if (count < 4)
                return false;
            else
                return true;
        }

        public bool ValidarId(Guid id)
        {
            return appDbContext.Reservas.Any(x => x.Id == id);
        }
    }
}