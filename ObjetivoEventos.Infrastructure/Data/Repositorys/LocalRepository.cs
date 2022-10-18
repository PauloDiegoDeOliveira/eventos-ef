using Microsoft.EntityFrameworkCore;
using ObjetivoEventos.Domain.Core.Interfaces.Repositories;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using ObjetivoEventos.Infrastructure.Data.Repositorys.Base;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ObjetivoEventos.Infrastructure.Data.Repositorys
{
    public class LocalRepository : RepositoryBase<Local>, ILocalRepository
    {
        private readonly AppDbContext appDbContext;

        public LocalRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<PagedList<Local>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            IQueryable<Local> locais = appDbContext.Set<Local>();

            if (parametersPalavraChave.PalavraChave == null && parametersPalavraChave.Id == null && parametersPalavraChave.Status == 0)
                locais = locais.Where(x => x.Status != Status.Excluido.ToString());
            else if (parametersPalavraChave.Status != 0)
                locais = locais.Where(x => x.Status == parametersPalavraChave.Status.ToString());

            if (!string.IsNullOrEmpty(parametersPalavraChave.PalavraChave))
                locais = locais.Where(x => EF.Functions.Like(x.Nome, $"%{parametersPalavraChave.PalavraChave}%"));

            if (parametersPalavraChave.Id != null)
                locais = locais.Where(x => parametersPalavraChave.Id.Contains(x.Id));

            locais = locais.OrderBy(x => x.CriadoEm)
                             .AsNoTracking();

            return await Task.FromResult(PagedList<Local>.ToPagedList(locais, parametersPalavraChave.NumeroPagina, parametersPalavraChave.ResultadosExibidos));
        }

        public bool ValidarId(Guid id)
        {
            return appDbContext.Locais.Any(x => x.Id == id);
        }

        public bool ValidarNome(Local local)
        {
            if (local.Id == Guid.Empty)
                return appDbContext.Locais
                              .AsNoTracking()
                              .Any(x => x.Nome.ToLower() == local.Nome.ToLower() && x.Status != Status.Excluido.ToString());
            else
                return appDbContext.Locais
                               .AsNoTracking()
                               .Any(x => x.Nome.ToLower() == local.Nome.ToLower() && x.Id != local.Id && x.Status != Status.Excluido.ToString());
        }

        public bool ExisteLocalId(Guid localId)
        {
            return appDbContext.Locais
                .AsNoTracking()
                .Any(Local => Local.Id == localId);
        }
    }
}