using ObjetivoEventos.Domain.Core.Interfaces.Repositories;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Pagination;
using ObjetivoEventos.Identity.Entitys;
using System;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Service
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            this.usuarioRepository = usuarioRepository;
        }

        public async Task<PagedList<Usuario>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            return await usuarioRepository.GetPaginationAsync(parametersPalavraChave);
        }

        public async Task<Usuario> GetByIdAsync(Guid id)
        {
            return await usuarioRepository.GetByIdAsync(id);
        }

        public async Task<Usuario> DeleteAsync(Guid id)
        {
            return await usuarioRepository.DeleteAsync(id);
        }

        public bool ValidarId(Guid id)
        {
            return usuarioRepository.ValidarId(id);
        }
    }
}