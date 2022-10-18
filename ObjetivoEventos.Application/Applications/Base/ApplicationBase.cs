using AutoMapper;
using ObjetivoEventos.Application.Dtos.Base;
using ObjetivoEventos.Application.Interfaces.Base;
using ObjetivoEventos.Application.Structs;
using ObjetivoEventos.Domain.Core.Interfaces.Service;
using ObjetivoEventos.Domain.Core.Interfaces.Service.Base;
using ObjetivoEventos.Domain.Core.Notificacoes;
using ObjetivoEventos.Domain.Entitys.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObjetivoEventos.Application.Applications.Base
{
    public class ApplicationBase<TEntity, TView, TPost, TPut, TStatus> :
         IApplicationBase<TEntity, TView, TPost, TPut, TStatus>
         where TEntity : EntityBase where TView : class where TPost : class where TPut : class where TStatus : PutStatusDto
    {
        protected readonly IMapper mapper;
        protected readonly IServiceBase<TEntity> serviceBase;
        private readonly INotificador notificador;

        public ApplicationBase(IServiceBase<TEntity> serviceBase, INotificador notificador, IMapper mapper)
        {
            this.serviceBase = serviceBase;
            this.notificador = notificador;
            this.mapper = mapper;
        }

        protected void Notificar(string mensagem)
        {
            notificador.Handle(new Notificacao(mensagem));
        }

        public virtual async Task<IEnumerable<TView>> GetAllAsync()
        {
            IEnumerable<TEntity> consulta = await serviceBase.GetAllAsync();
            return mapper.Map<IList<TView>>(consulta);
        }

        public virtual async Task<TView> GetByIdAsync(Guid id)
        {
            TEntity consulta = await serviceBase.GetByIdAsync(id);
            return mapper.Map<TView>(consulta);
        }

        public virtual async Task<EntityToDto<TEntity, TPut>> MapStructById(Guid id)
        {
            TEntity obj = await serviceBase.GetByIdAsync(id);
            return new EntityToDto<TEntity, TPut>(obj, mapper.Map<TPut>(obj));
        }

        public virtual async Task<TView> PostAsync(TPost post)
        {
            TEntity consulta = mapper.Map<TEntity>(post);
            consulta = await serviceBase.PostAsync(consulta);
            return mapper.Map<TView>(consulta);
        }

        public virtual async Task<TView> PutAsync(TPut put)
        {
            TEntity consulta = mapper.Map<TEntity>(put);
            consulta = await serviceBase.PutAsync(consulta);

            if (consulta is null)
                return null;

            return mapper.Map<TView>(consulta);
        }

        public virtual async Task<TView> DeleteAsync(Guid id)
        {
            TEntity consulta = await serviceBase.DeleteAsync(id);
            return mapper.Map<TView>(consulta);
        }

        public virtual async Task<TView> PutStatusAsync(TStatus status)
        {
            TEntity consulta = await serviceBase.GetByIdAsync(status.Id);

            if (consulta is null)
                return null;

            consulta.ChangeStatusValue(status.Status.ToString());

            TEntity obj = await serviceBase.PutStatusAsync(consulta);
            return mapper.Map<TView>(obj);
        }

        public virtual async Task<bool> ExisteNaBaseAsync(Guid id)
        {
            return await serviceBase.ExisteNaBaseAsync(id);
        }

        public virtual async Task MapStructSaveChangesAsync(EntityToDto<TEntity, TPut> dtoStruct)
        {
            mapper.Map(dtoStruct.Dto, dtoStruct.Entity);
            await serviceBase.SaveChangesAsync();
        }
    }
}