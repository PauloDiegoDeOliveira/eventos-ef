using ObjetivoEventos.Domain.Core.Interfaces.Repositories;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Pagination;
using ObjetivoEventos.Domain.Parameters;
using ObjetivoEventos.Domain.Service.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Service
{
    public class SetorService : ServiceBase<Setor>, ISetorService
    {
        private readonly ISetorRepository setorRepository;

        public SetorService(ISetorRepository setorRepository) : base(setorRepository)
        {
            this.setorRepository = setorRepository;
        }

        public async Task<PagedList<Setor>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            return await setorRepository.GetPaginationAsync(parametersPalavraChave);
        }

        public async Task<List<Setor>> GetSetorByReservas(List<Guid> reservasId)
        {
            return await setorRepository.GetSetorByReservas(reservasId);
        }

        public async Task<Setor> PostSetorByCadeirasAutomaticoAsync(Setor setor, List<GridParameters> gridParameters)
        {
            return await setorRepository.PostSetorByCadeirasAutomaticoAsync(setor, gridParameters);
        }

        public async Task<Setor> PutSetorByCadeirasAutomaticoAsync(Setor setor, List<GridParameters> gridParameters)
        {
            return await setorRepository.PutSetorByCadeirasAutomaticoAsync(setor, gridParameters);
        }

        public bool ValidarId(Guid id)
        {
            return setorRepository.ValidarId(id);
        }
    }
}