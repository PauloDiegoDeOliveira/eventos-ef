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
    public class CadeiraRepository : RepositoryBase<Cadeira>, ICadeiraRepository
    {
        private readonly AppDbContext appDbContext;

        public CadeiraRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<PagedList<Cadeira>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            IQueryable<Cadeira> cadeiras = appDbContext.Set<Cadeira>();

            if (parametersPalavraChave.PalavraChave == null && parametersPalavraChave.Id == null && parametersPalavraChave.Status == 0)
                cadeiras = cadeiras.Where(x => x.Status != Status.Excluido.ToString());
            else if (parametersPalavraChave.Status != 0)
                cadeiras = cadeiras.Where(x => x.Status == parametersPalavraChave.Status.ToString());

            if (!string.IsNullOrEmpty(parametersPalavraChave.PalavraChave))
                cadeiras = cadeiras.Where(x => EF.Functions.Like(x.Nome, $"%{parametersPalavraChave.PalavraChave}%"));

            if (parametersPalavraChave.Id != null)
                cadeiras = cadeiras.Where(x => parametersPalavraChave.Id.Contains(x.Id));

            cadeiras = cadeiras.OrderBy(x => x.Fileira).ThenBy(x => x.Coluna).AsNoTracking();

            return await Task.FromResult(PagedList<Cadeira>.ToPagedList(cadeiras, parametersPalavraChave.NumeroPagina, parametersPalavraChave.ResultadosExibidos));
        }

        public async Task<List<Cadeira>> GetByColunaLinhaAsync(List<Cadeira> cadeiras)
        {
            return await appDbContext.Cadeiras
              .Where(cadeira => cadeiras.Select(coluna => coluna.Fileira).Contains(cadeira.Fileira)
                   && cadeiras.Select(linha => linha.Coluna).Contains(cadeira.Coluna))
              .ToListAsync();
        }

        public async Task<List<Cadeira>> PostAutomaticoAsync(List<Cadeira> cadeiras)
        {
            appDbContext.AddRange(cadeiras);
            await appDbContext.SaveChangesAsync();
            return cadeiras;
        }

        public bool ValidarId(Guid id)
        {
            return appDbContext.Cadeiras.Any(x => x.Id == id);
        }

        public bool ValidarFileiraEColuna(Cadeira cadeira)
        {
            if (cadeira.Id == Guid.Empty)
                return appDbContext.Cadeiras
                              .AsNoTracking()
                              .Any(x => x.Fileira.ToLower() == cadeira.Fileira.ToLower() && x.Coluna == cadeira.Coluna && x.Status != Status.Excluido.ToString());
            else
                return appDbContext.Cadeiras
                               .AsNoTracking()
                               .Any(x => x.Fileira.ToLower() == cadeira.Fileira.ToLower() && x.Coluna == cadeira.Coluna && x.Id != cadeira.Id && x.Status != Status.Excluido.ToString());
        }
    }
}