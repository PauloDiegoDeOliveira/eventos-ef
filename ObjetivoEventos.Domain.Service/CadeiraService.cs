using ObjetivoEventos.Domain.Core.Interfaces.Repositories;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Pagination;
using ObjetivoEventos.Domain.Service.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Service
{
    public class CadeiraService : ServiceBase<Cadeira>, ICadeiraService
    {
        private readonly ICadeiraRepository cadeiraRepository;

        public CadeiraService(ICadeiraRepository cadeiraRepository) : base(cadeiraRepository)
        {
            this.cadeiraRepository = cadeiraRepository;
        }

        public async Task<PagedList<Cadeira>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            return await cadeiraRepository.GetPaginationAsync(parametersPalavraChave);
        }

        public async Task<List<Cadeira>> PostAutomaticoAsync(List<Cadeira> cadeiras)
        {
            List<Cadeira> result = await cadeiraRepository.GetByColunaLinhaAsync(cadeiras);
            if (result is not null)
                cadeiras.RemoveAll(x => result.Any(y => y.Fileira.ToLower() == x.Fileira.ToLower() && y.Coluna == x.Coluna));

            return await cadeiraRepository.PostAutomaticoAsync(cadeiras);
        }

        public bool ValidarId(Guid id)
        {
            return cadeiraRepository.ValidarId(id);
        }

        public bool ValidarFileiraEColuna(Cadeira cadeira)
        {
            return cadeiraRepository.ValidarFileiraEColuna(cadeira);
        }
    }
}