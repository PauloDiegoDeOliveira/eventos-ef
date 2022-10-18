using ObjetivoEventos.Domain.Core.Interfaces.Repositories;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Entitys;
using ObjetivoEventos.Domain.Pagination;
using ObjetivoEventos.Domain.Service.Base;
using System;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Service
{
    public class MesaService : ServiceBase<Mesa>, IMesaService
    {
        private readonly IMesaRepository mesaRepository;

        public MesaService(IMesaRepository mesaRepository) : base(mesaRepository)
        {
            this.mesaRepository = mesaRepository;
        }

        public async Task<PagedList<Mesa>> GetPaginationAsync(ParametersPalavraChave parametersPalavraChave)
        {
            return await mesaRepository.GetPaginationAsync(parametersPalavraChave);
        }

        public bool ValidaCadeiraRegistrada(Mesa mesa)
        {
            return mesaRepository.ValidaCadeiraRegistrada(mesa);
        }

        public bool ValidarId(Guid id)
        {
            return mesaRepository.ValidarId(id);
        }

        public bool ValidarFileiraEColuna(Mesa mesa)
        {
            return mesaRepository.ValidarFileiraEColuna(mesa);
        }
    }
}