using Microsoft.EntityFrameworkCore;
using ObjetivoEventos.Domain.Core.Interfaces.Repositories;
using ObjetivoEventos.Domain.Enums;
using ObjetivoEventos.Domain.Pagination;
using ObjetivoEventos.Identity.Data;
using ObjetivoEventos.Identity.Entitys;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ObjetivoEventos.Infrastructure.Data.Repositorys
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly IdentityDataContext identityDataContext;

        public UsuarioRepository(IdentityDataContext identityDataContext)
        {
            this.identityDataContext = identityDataContext;
        }

        public async Task<PagedList<Usuario>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            IQueryable<Usuario> usuario = identityDataContext.Set<Usuario>();

            if (parametersPalavraChave.PalavraChave == null && parametersPalavraChave.Id == null && parametersPalavraChave.Status == 0)
                usuario = usuario.Where(x => x.Status != Status.Excluido.ToString());
            else if (parametersPalavraChave.Status != 0)
                usuario = usuario.Where(x => x.Status == parametersPalavraChave.Status.ToString());

            if (!string.IsNullOrEmpty(parametersPalavraChave.PalavraChave))
                usuario = usuario.Where(x => EF.Functions.Like(x.Nome, $"%{parametersPalavraChave.PalavraChave}%"));

            if (parametersPalavraChave.Id != null)
                usuario = usuario.Where(x => parametersPalavraChave.Id.Contains(Guid.Parse(x.Id)));

            return await Task.FromResult(PagedList<Usuario>.ToPagedList(usuario, parametersPalavraChave.NumeroPagina, parametersPalavraChave.ResultadosExibidos));
        }

        public async Task<Usuario> GetByIdAsync(Guid id)
        {
            return await identityDataContext.Users.FindAsync(id.ToString());
        }

        public async Task<Usuario> DeleteAsync(Guid id)
        {
            Usuario consultado = await identityDataContext.Users.FindAsync(id.ToString());

            if (consultado is null)
                return null;

            var removido = identityDataContext.Remove(consultado);
            await identityDataContext.SaveChangesAsync();

            return removido.Entity;
        }

        public bool ValidarId(Guid id)
        {
            return identityDataContext.Users.Any(x => x.Id == id.ToString());
        }
    }
}