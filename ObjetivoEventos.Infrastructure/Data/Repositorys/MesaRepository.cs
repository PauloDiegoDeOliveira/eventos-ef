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
    public class MesaRepository : RepositoryBase<Mesa>, IMesaRepository
    {
        private readonly AppDbContext appDbContext;

        public MesaRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<PagedList<Mesa>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            IQueryable<Mesa> Mesas = appDbContext.Set<Mesa>();

            if (parametersPalavraChave.PalavraChave == null && parametersPalavraChave.Id == null && parametersPalavraChave.Status == 0)
                Mesas = Mesas.Where(x => x.Status != Status.Excluido.ToString());
            else if (parametersPalavraChave.Status != 0)
                Mesas = Mesas.Where(x => x.Status == parametersPalavraChave.Status.ToString());

            if (!string.IsNullOrEmpty(parametersPalavraChave.PalavraChave))
                Mesas = Mesas.Where(x => EF.Functions.Like(x.Nome, $"%{parametersPalavraChave.PalavraChave}%"));

            if (parametersPalavraChave.Id != null)
                Mesas = Mesas.Where(x => parametersPalavraChave.Id.Contains(x.Id));

            Mesas = Mesas.OrderBy(x => x.CriadoEm).AsNoTracking();

            return await Task.FromResult(PagedList<Mesa>.ToPagedList(Mesas, parametersPalavraChave.NumeroPagina, parametersPalavraChave.ResultadosExibidos));
        }

        public override async Task<Mesa> PostAsync(Mesa mesa)
        {
            return await base.PostAsync(await InsertMesaAsync(mesa));
        }

        private async Task<Mesa> InsertMesaAsync(Mesa mesa)
        {
            await InsertCadeirasAsync(mesa);

            return mesa;
        }

        private async Task InsertCadeirasAsync(Mesa mesa)
        {
            List<Cadeira> cadeirasConsultadas = new List<Cadeira>();

            foreach (Cadeira cadeira in mesa.Cadeiras)
            {
                Cadeira cadeiraConsultada = await appDbContext.Cadeiras.FindAsync(cadeira.Id);
                cadeirasConsultadas.Add(cadeiraConsultada);
            }

            mesa.ListaCadeiras(cadeirasConsultadas);
        }

        public override async Task<Mesa> PutAsync(Mesa mesa)
        {
            return await base.PutAsync(await UpdateMesaAsync(mesa));
        }

        private async Task<Mesa> UpdateMesaAsync(Mesa mesa)
        {
            Mesa mesaConsultada = await appDbContext.Mesas
                                                 .Include(p => p.Cadeiras)
                                                 .FirstOrDefaultAsync(p => p.Id == mesa.Id);
            if (mesaConsultada == null)
                return null;

            await UpdateCadeirasAsync(mesa, mesaConsultada);

            return mesaConsultada;
        }

        private async Task UpdateCadeirasAsync(Mesa mesa, Mesa mesaConsultado)
        {
            mesaConsultado.Cadeiras.Clear();
            foreach (Cadeira cadeira in mesa.Cadeiras)
            {
                Cadeira cadeiraConsultada = await appDbContext.Cadeiras.FindAsync(cadeira.Id);
                mesaConsultado.Cadeiras.Add(cadeiraConsultada);
            }
        }

        public bool ValidarId(Guid id)
        {
            return appDbContext.Mesas.Any(x => x.Id == id);
        }

        public bool ValidaCadeiraRegistrada(Mesa mesa)
        {
            Mesa consulta = appDbContext.Mesas
                          .AsNoTracking()
                          .Include(p => p.Cadeiras)
                          .FirstOrDefault(p => p.Id == mesa.Id);

            return consulta.Cadeiras.Any(e => mesa.Cadeiras.Any(x => x.Id == e.Id));
        }

        public bool ValidarFileiraEColuna(Mesa mesa)
        {
            if (mesa.Id == Guid.Empty)
                return appDbContext.Mesas
                              .AsNoTracking()
                              .Any(x => x.Fileira.ToLower() == mesa.Fileira.ToLower() && mesa.Coluna == mesa.Coluna && x.Status != Status.Excluido.ToString());
            else
                return appDbContext.Mesas
                              .AsNoTracking()
                               .Any(x => x.Fileira.ToLower() == mesa.Fileira.ToLower() && x.Coluna == mesa.Coluna && x.Id != mesa.Id && x.Status != Status.Excluido.ToString());
        }
    }
}