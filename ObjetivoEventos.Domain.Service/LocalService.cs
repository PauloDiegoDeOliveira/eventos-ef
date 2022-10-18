using ObjetivoEventos.Domain.Core.Interfaces.Repositories;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Pagination;
using ObjetivoEventos.Domain.Service.Base;
using System;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Service
{
    public class LocalService : ServiceBase<Local>, ILocalService
    {
        private readonly ILocalRepository localRepository;

        public LocalService(ILocalRepository localRepository) : base(localRepository)
        {
            this.localRepository = localRepository;
        }

        public async Task<PagedList<Local>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            return await localRepository.GetPaginationAsync(parametersPalavraChave);
        }

        public bool ValidarId(Guid id)
        {
            return localRepository.ValidarId(id);
        }

        public bool ValidarNome(Local local)
        {
            return localRepository.ValidarNome(local);
        }

        public bool ExisteLocalId(Guid localId)
        {
            return localRepository.ExisteLocalId(localId);
        }
    }
}