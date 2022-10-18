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
    public class PedidoRepository : RepositoryBase<Pedido>, IPedidoRepository
    {
        private readonly AppDbContext appDbContext;
        private readonly IUser user;

        public PedidoRepository(AppDbContext appDbContext,
                                IUser user) : base(appDbContext)
        {
            this.appDbContext = appDbContext;
            this.user = user;
        }

        public async Task<PagedList<Pedido>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            IQueryable<Pedido> pedidos = appDbContext.Set<Pedido>();

            if (parametersPalavraChave.PalavraChave == null && parametersPalavraChave.Id == null && parametersPalavraChave.Status == 0)
                pedidos = pedidos.Where(x => x.Status != Status.Excluido.ToString());
            else if (parametersPalavraChave.Status != 0)
                pedidos = pedidos.Where(x => x.Status == parametersPalavraChave.Status.ToString());

            if (!string.IsNullOrEmpty(parametersPalavraChave.PalavraChave))
                pedidos = pedidos.Where(x => EF.Functions.Like(x.SituacaoPedido, $"%{parametersPalavraChave.PalavraChave}%"));

            if (parametersPalavraChave.Id != null)
                pedidos = pedidos.Where(x => parametersPalavraChave.Id.Contains(x.Id));

            pedidos = pedidos.OrderBy(x => x.CriadoEm).AsNoTracking();

            return await Task.FromResult(PagedList<Pedido>.ToPagedList(pedidos, parametersPalavraChave.NumeroPagina, parametersPalavraChave.ResultadosExibidos));
        }

        public async Task<Pedido> GetDetalhesByIdAsync(Guid id)
        {
            return await appDbContext.Set<Pedido>()
                                     .AsNoTracking()
                                     .Include(x => x.Reservas)
                                     .ThenInclude(x => x.Evento)
                                     .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Pedido>> GetByUsuarioAutenticadoAsync()
        {
            return await appDbContext.Set<Pedido>()
                                     .AsNoTracking()
                                     .Where(x => x.UsuarioId == user.GetUserId())
                                     .Include(x => x.Reservas)
                                     .ThenInclude(x => x.Evento)
                                     .ThenInclude(x => x.Local)
                                     .ToListAsync();
        }

        public async Task<List<Pedido>> GetPedidosAtivosSituacaoPedidoAsync(SituacaoPedido situacaoPedido)
        {
            //TODO: Se não funcionar voltar com a validação de data atual CriadoEm.Date < DateTime.Today
            return await appDbContext.Pedidos.Where(x => x.SituacaoPedido == situacaoPedido.ToString()
                && x.Status == Status.Ativo.ToString())
               .AsNoTracking()
               .Include(x => x.Reservas)
               .ToListAsync();
        }

        public async Task<List<Pedido>> GetPedidosVencidosPorDiasAsync(int dias)
        {
            return await appDbContext.Pedidos.Where(x => x.SituacaoPedido == SituacaoPedido.AguardandoPagamento.ToString()
                && x.CriadoEm.AddDays(dias) < DateTime.Now
                && x.Status == Status.Ativo.ToString())
               .AsNoTracking()
               .Include(x => x.Reservas)
               .ToListAsync();
        }

        public async Task<List<Pedido>> VerificaPedidosVencidosPeloEventoAsync(bool pedidosVencidos, List<Pedido> pedidos)
        {
            List<Evento> eventos = await appDbContext.Eventos.Where(x => x.DataEvento.Date < DateTime.Today).ToListAsync();

            if (eventos is null)
                return null;

            if (pedidosVencidos)
                //Retorna os que o evento já aconteceu
                return pedidos.Where(pedido => eventos.Any(evento => pedido.Reservas.Any(reserva => reserva.EventoId == evento.Id))).ToList();
            else
                //Retorna os que o evento ainda não aconteceu
                return pedidos.Where(pedido => !eventos.Any(evento => pedido.Reservas.Any(reserva => reserva.EventoId == evento.Id))).ToList();
        }

        public override async Task<Pedido> PostAsync(Pedido pedido)
        {
            pedido.AlteraUsuarioId(user.GetUserId());
            return await base.PostAsync(await InsertPedidoAsync(pedido));
        }

        private async Task<Pedido> InsertPedidoAsync(Pedido pedido)
        {
            await InsertReservasAsync(pedido);
            return pedido;
        }

        private async Task InsertReservasAsync(Pedido pedido)
        {
            List<Reserva> reservasConsultadas = new();

            foreach (Reserva reserva in pedido.Reservas)
            {
                Reserva reservaConsultada = await appDbContext.Reservas.FindAsync(reserva.Id);
                reservasConsultadas.Add(reservaConsultada);
            }

            List<Setor> setorConsultado = reservasConsultadas.Select(reserva => appDbContext.Setores.Find(reserva.SetorId)).ToList();

            float precoTotal = 0;
            foreach (Setor setor in setorConsultado)
                precoTotal += setor.Preco;

            pedido.AlterarValorTotal(precoTotal);

            pedido.AlterarReservas(reservasConsultadas);
        }

        public override async Task<Pedido> PutAsync(Pedido pedido)
        {
            Pedido consulta = await GetByIdAsync(pedido.Id);
            pedido.AlteraUsuarioId(consulta.UsuarioId);
            pedido.AlterarSituacaoPedido(consulta.SituacaoPedido);
            return await base.PutAsync(await UpdatePedidoAsync(pedido));
        }

        private async Task<Pedido> UpdatePedidoAsync(Pedido pedido)
        {
            Pedido pedidoConsultado = await appDbContext.Pedidos
                                                 .Include(x => x.Reservas)
                                                 .FirstOrDefaultAsync(x => x.Id == pedido.Id);

            if (pedidoConsultado is null)
                return null;

            await UpdateReservasAsync(pedido, pedidoConsultado);

            List<Setor> setorConsultado = pedidoConsultado.Reservas.Select(reserva => appDbContext.Setores.Find(reserva.SetorId)).ToList();

            float precoTotal = 0;
            foreach (Setor setor in setorConsultado)
                precoTotal += setor.Preco;

            pedido.AlterarValorTotal(precoTotal);

            appDbContext.Entry(pedidoConsultado).CurrentValues.SetValues(pedido);

            return pedidoConsultado;
        }

        private async Task UpdateReservasAsync(Pedido pedido, Pedido pedidoConsultado)
        {
            string situacaoReservas = pedidoConsultado.Reservas[0].SituacaoReserva;
            string statusReservas = pedidoConsultado.Reservas[0].Status;

            Guid[] reservaIds = pedido.Reservas.Select(x => x.Id).ToArray();
            List<Reserva> reservasRemover = pedidoConsultado.Reservas.Where(reserva => !reservaIds.Contains(reserva.Id)).ToList();
            appDbContext.Reservas.RemoveRange(reservasRemover);

            pedidoConsultado.Reservas.Clear();

            foreach (Reserva reserva in pedido.Reservas)
            {
                Reserva reservaConsultada = await appDbContext.Reservas.FindAsync(reserva.Id);
                reservaConsultada.AlterarSituacaoReserva(situacaoReservas);
                reservaConsultada.ChangeStatusValue(statusReservas);
                pedidoConsultado.Reservas.Add(reservaConsultada);
            }
        }

        public async Task<Pedido> PutTrocaPedidoReserva(Guid pedidoId, Guid reservaAntigaId, Guid novaReservaId)
        {
            Pedido pedido = await appDbContext.Set<Pedido>()
                                      .Include(x => x.Reservas)
                                      .ThenInclude(x => x.Evento)
                                      .FirstOrDefaultAsync(x => x.Id == pedidoId);

            if (pedido == null)
                return null;

            if (pedido.Reservas.Count <= 0)
                return null;

            Reserva reservaAntiga = pedido.Reservas.Where(x => x.Id == reservaAntigaId).FirstOrDefault();

            if (reservaAntiga == null)
                return null;

            Reserva novaReserva = await appDbContext.Set<Reserva>()
                                     .FirstOrDefaultAsync(x => x.Id == novaReservaId);

            if (novaReserva == null)
                return null;

            novaReserva.AlterarSituacaoReserva(reservaAntiga.SituacaoReserva);

            pedido.Reservas.Remove(reservaAntiga);
            pedido.Reservas.Add(novaReserva);

            appDbContext.Remove(reservaAntiga);
            appDbContext.Update(pedido);
            appDbContext.SaveChanges();

            return pedido;
        }

        public async Task<Pedido> PutSituacaoPedidoAsync(Pedido pedido)
        {
            return await base.PutAsync(pedido);
        }

        public async Task<List<Pedido>> PutPedidosAtivosParaInativosByEventosExpirados(List<Evento> eventos)
        {
            List<Pedido> pedidos = await appDbContext.Pedidos.Where(x => x.Status == Status.Ativo.ToString())
                        .Include(x => x.Reservas).ToListAsync();

            if (pedidos is null)
                return null;

            List<Pedido> pedidosRetorno = pedidos.Where(pedido => eventos.Any(evento => pedido.Reservas.Any(reserva => reserva.EventoId == evento.Id))).ToList();

            if (pedidosRetorno is null)
                return null;

            foreach (Pedido pedido in pedidosRetorno)
                pedido.ChangeStatusValue(Status.Inativo.ToString());

            appDbContext.UpdateRange(pedidosRetorno);
            await appDbContext.SaveChangesAsync();
            return pedidosRetorno;
        }

        public bool ValidarId(Guid id)
        {
            return appDbContext.Pedidos.Any(x => x.Id == id);
        }

        public bool ValidarInputReservaEventoPedidos(Pedido pedido)
        {
            Guid[] reservaIds = pedido.Reservas.Select(x => x.Id).ToArray();

            List<Reserva> reservasConsulta = appDbContext.Reservas.AsNoTracking()
                                                .Where(x => reservaIds.Contains(x.Id)).ToList();

            IEnumerable<IGrouping<Guid, Reserva>> grupoConsulta = reservasConsulta.GroupBy(x => x.EventoId);

            if (grupoConsulta.Count() > 1)
                return false;

            return true;
        }

        public bool ValidarReservaPedidos(Pedido pedido)
        {
            Guid[] reservaIds = pedido.Reservas.Select(x => x.Id).ToArray();

            IQueryable<Pedido> pedidosConsulta;
            if (pedido.Id != Guid.Empty)
                pedidosConsulta = appDbContext.Pedidos.Where(x => x.Id != pedido.Id && x.Reservas.Any(reserva => reservaIds.Contains(reserva.Id)));
            else
                pedidosConsulta = appDbContext.Pedidos.Where(x => x.Reservas.Any(reserva => reservaIds.Contains(reserva.Id)));

            if (pedidosConsulta.Any())
                return true;

            return false;
        }
    }
}