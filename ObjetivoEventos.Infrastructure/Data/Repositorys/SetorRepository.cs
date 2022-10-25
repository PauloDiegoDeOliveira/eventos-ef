using Microsoft.EntityFrameworkCore;
using ObjetivoEventos.Domain.Core.Interfaces.Repositories;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using ObjetivoEventos.Domain.Parameters;
using ObjetivoEventos.Infrastructure.Data.Repositorys.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjetivoEventos.Infrastructure.Data.Repositorys
{
    public class SetorRepository : RepositoryBase<Setor>, ISetorRepository
    {
        private readonly AppDbContext appDbContext;

        public SetorRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<PagedList<Setor>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            IQueryable<Setor> Setores = appDbContext.Set<Setor>();

            if (parametersPalavraChave.PalavraChave == null && parametersPalavraChave.Id == null && parametersPalavraChave.Status == 0)
                Setores = Setores.Where(x => x.Status != Status.Excluido.ToString());
            else if (parametersPalavraChave.Status != 0)
                Setores = Setores.Where(x => x.Status == parametersPalavraChave.Status.ToString());

            if (!string.IsNullOrEmpty(parametersPalavraChave.PalavraChave))
                Setores = Setores.Where(x => EF.Functions.Like(x.Nome, $"%{parametersPalavraChave.PalavraChave}%"));

            if (parametersPalavraChave.Id != null)
                Setores = Setores.Where(x => parametersPalavraChave.Id.Contains(x.Id));

            Setores = Setores.OrderBy(x => x.CriadoEm).AsNoTracking();

            return await Task.FromResult(PagedList<Setor>.ToPagedList(Setores, parametersPalavraChave.NumeroPagina, parametersPalavraChave.ResultadosExibidos));
        }

        public async Task<List<Setor>> GetSetoresByListIdNoTracking(List<Guid> ids)
        {
            return await appDbContext.Setores.AsNoTracking()
                         .Where(setor => ids.Contains(setor.Id))
                         .ToListAsync();
        }

        public async Task<List<Setor>> GetSetorByReservas(List<Guid> reservasId)
        {
            List<Reserva> reservas = await appDbContext.Reservas.Where(x => reservasId.Contains(x.Id)).ToListAsync();
            IEnumerable<IGrouping<Guid, Reserva>> reservaGroups = reservas.GroupBy(x => x.SetorId);

            List<Setor> setores = new();
            foreach (IGrouping<Guid, Reserva> reservaGroup in reservaGroups)
            {
                List<Setor> dadosSetor = new();
                foreach (Reserva reserva in reservaGroup)
                {
                    if (reserva.MesaId != null)
                        dadosSetor.Add(await appDbContext.Setores.AsNoTracking()
                            .Include(x => x.Mesas.Where(x => x.Id == reserva.MesaId))
                            .ThenInclude(x => x.Cadeiras.Where(x => x.Id == reserva.CadeiraId))
                            .FirstOrDefaultAsync(x => x.Id == reserva.SetorId));
                    else
                        dadosSetor.Add(await appDbContext.Setores.AsNoTracking()
                            .Include(x => x.Cadeiras.Where(x => x.Id == reserva.CadeiraId))
                            .FirstOrDefaultAsync(x => x.Id == reserva.SetorId));
                }

                foreach (Setor setor in dadosSetor)
                {
                    if (dadosSetor.IndexOf(setor) > 0)
                    {
                        if (setor.Mesas != null)
                            dadosSetor[0].Mesas.AddRange(setor.Mesas);

                        dadosSetor[0].Cadeiras.AddRange(setor.Cadeiras);
                    }
                }

                setores.Add(dadosSetor[0]);
            }

            return setores;
        }

        public async Task<Setor> PostSetorByCadeirasAutomaticoAsync(Setor setor, List<GridParameters> gridParameters)
        {
            List<Cadeira> listaCadeiras = await appDbContext.Cadeiras
                .Where(cadeira => gridParameters.Select(fileira => fileira.Fileira.ToString()).Contains(cadeira.Fileira)).ToListAsync();

            if (listaCadeiras is null)
                return null;

            List<Cadeira> finalList = new();
            foreach (Cadeira cadeira in listaCadeiras)
                if (cadeira.Coluna <= gridParameters[gridParameters.IndexOf(gridParameters.FirstOrDefault(x => x.Fileira.ToString() == cadeira.Fileira))].Coluna)
                    finalList.Add(cadeira);

            setor.ListaCadeiras(finalList);

            appDbContext.Setores.Add(setor);
            await appDbContext.SaveChangesAsync();

            return setor;
        }

        public async Task<Setor> PutSetorByCadeirasAutomaticoAsync(Setor setor, List<GridParameters> gridParameters)
        {
            Setor setorConsultado = await appDbContext.Setores
                .Include(x => x.Cadeiras)
                .FirstOrDefaultAsync(x => x.Id == setor.Id);

            if (setorConsultado is null)
                return null;

            IQueryable<Cadeira> cadeiras = appDbContext.Set<Cadeira>();

            cadeiras = cadeiras.Where(cadeira => gridParameters.Select(x => x.Fileira.ToString()).Contains(cadeira.Fileira));

            List<Cadeira> listaCadeiras = new();
            foreach (Cadeira cadeira in cadeiras)
                if (cadeira.Coluna <= gridParameters[gridParameters.IndexOf(gridParameters.FirstOrDefault(x => x.Fileira.ToString() == cadeira.Fileira))].Coluna)
                    listaCadeiras.Add(cadeira);

            cadeiras = listaCadeiras.OrderBy(x => x.Fileira).ThenBy(x => x.Coluna).AsQueryable();
            listaCadeiras = cadeiras.ToList();
            setorConsultado.ListaCadeiras(listaCadeiras);

            appDbContext.Entry(setorConsultado).CurrentValues.SetValues(setor);
            await appDbContext.SaveChangesAsync();

            return setorConsultado;
        }

        public override async Task<Setor> PostAsync(Setor setor)
        {
            return await base.PostAsync(await InsertSetorAsync(setor));
        }

        private async Task<Setor> InsertSetorAsync(Setor setor)
        {
            await InsertMesasAsync(setor);
            await InsertCadeirasAsync(setor);
            return setor;
        }

        private async Task InsertMesasAsync(Setor setor)
        {
            List<Mesa> mesasConsultadas = new();
            foreach (Mesa mesa in setor.Mesas)
            {
                Mesa mesaConsultada = await appDbContext.Mesas
                    .Include(x => x.Cadeiras)
                    .FirstOrDefaultAsync(x => x.Id == mesa.Id);
                mesasConsultadas.Add(mesaConsultada);
            }

            setor.ListaMesas(mesasConsultadas);
        }

        private async Task InsertCadeirasAsync(Setor setor)
        {
            List<Cadeira> cadeirasConsultadas = new();
            foreach (Cadeira cadeira in setor.Cadeiras)
            {
                Cadeira cadeiraConsultada = await appDbContext.Cadeiras.FindAsync(cadeira.Id);
                cadeirasConsultadas.Add(cadeiraConsultada);
            }

            setor.ListaCadeiras(cadeirasConsultadas);
        }

        public override async Task<Setor> PutAsync(Setor setor)
        {
            return await base.PutAsync(await UpdateSetorAsync(setor));
        }

        private async Task<Setor> UpdateSetorAsync(Setor setor)
        {
            Setor setorConsultado = await appDbContext.Setores
                                                 .Include(p => p.Mesas)
                                                 .Include(p => p.Cadeiras)
                                                 .FirstOrDefaultAsync(p => p.Id == setor.Id);
            if (setorConsultado == null)
                return null;

            appDbContext.Entry(setorConsultado).CurrentValues.SetValues(setor);
            await UpdateMesasAsync(setor, setorConsultado);
            await UpdateCadeirasAsync(setor, setorConsultado);
            return setorConsultado;
        }

        private async Task UpdateMesasAsync(Setor setor, Setor setorConsultado)
        {
            setorConsultado.Mesas.Clear();
            foreach (Mesa mesa in setor.Mesas)
            {
                Mesa mesaConsultada = await appDbContext.Mesas
                         .Include(p => p.Cadeiras)
                         .FirstOrDefaultAsync(x => x.Id == mesa.Id);
                setorConsultado.Mesas.Add(mesaConsultada);
            }
        }

        private async Task UpdateCadeirasAsync(Setor setor, Setor setorConsultado)
        {
            setorConsultado.Cadeiras.Clear();
            foreach (Cadeira cadeira in setor.Cadeiras)
            {
                Cadeira cadeiraConsultada = await appDbContext.Cadeiras.FindAsync(cadeira.Id);
                setorConsultado.Cadeiras.Add(cadeiraConsultada);
            }
        }

        public bool ValidarId(Guid id)
        {
            return appDbContext.Setores.Any(x => x.Id == id);
        }
    }
}