using ObjetivoEventos.Domain.Pagination;
using ObjetivoEventos.Identity.Entitys;
using System;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Core.Interfaces.Service
{
    public interface IUsuarioService
    {
        Task<PagedList<Usuario>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave);

        Task<Usuario> GetByIdAsync(Guid id);

        Task<Usuario> DeleteAsync(Guid id);

        bool ValidarId(Guid id);
    }
}