using ObjetivoEventos.Domain.Entitys.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Domain.Core.Interfaces.Repositories.Base
{
    public interface IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        Task<IEnumerable<TEntity>> GetAllAsync();

        Task<TEntity> GetByIdAsync(Guid id);

        Task<TEntity> PostAsync(TEntity obj);

        Task<TEntity> PutAsync(TEntity obj);

        Task<TEntity> PutStatusAsync(TEntity obj);

        Task<TEntity> DeleteAsync(Guid id);

        Task<bool> ExisteNaBaseAsync(Guid id);

        Task SaveChangesAsync();
    }
}