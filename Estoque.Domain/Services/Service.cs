using Estoque.Core.Entities;
using Estoque.Core.Interfaces;

namespace Estoque.Domain.Services
{
    public class Service<TEntity>: IService<TEntity> where TEntity : EntityBase
    {
        protected readonly IRepository<TEntity> _repository;
        protected readonly INotifiable _notifiable;
        protected readonly IAspnetUser _aspnetUser;
        protected readonly IUnitOfWork _unitOfWork;

        public Service(IRepository<TEntity> repository, INotifiable notifiable, IUnitOfWork unitOfWork, IAspnetUser aspnetUser)
        {
            _repository = repository;
            _notifiable = notifiable;
            _unitOfWork = unitOfWork;
            _aspnetUser = aspnetUser;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAsync() => await _repository.GetAsync();
        public virtual async Task<TEntity> GetAsync(Guid id) => await _repository.GetAsync(id);

        public virtual async Task<Guid> InsertAsync(TEntity entity)
        {
            await Validate(entity);

            if (_notifiable.HasNotification)
                return Guid.Empty;

            await _repository.InsertAsync(entity);
            await _unitOfWork.CommitAsync();

            return await Task.FromResult(entity.Id);
        }

        public virtual async Task UpdateAsync(Guid id, TEntity entity)
        {
            await Validate(entity);

            if (_notifiable.HasNotification)
                return;

            var entityDb = await _repository.GetAsync(id);

            var method = entityDb.GetType().GetMethod("Atualizar");

            if (method != null)
                method.Invoke(entityDb, new object[] { entity });
            else
                return;

            _repository.Update(entityDb);
            await _unitOfWork.CommitAsync();
        }

        public virtual async Task UpdateAsync(List<TEntity> entities)
        {
            foreach(var entity in entities)
            {
                await Validate(entity);

                if (_notifiable.HasNotification)
                    return;

                var method = entity.GetType().GetMethod("Atualizar");

                if (method != null)
                    method.Invoke(entity, new object[] { entity });
                else
                    return;

            }

            _repository.Update(entities);
            await _unitOfWork.CommitAsync();
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entityDb = await _repository.GetAsync(id);

            var method = entityDb.GetType().GetMethod("Remover");

            if (method != null)
            {
                method.Invoke(entityDb, null);

                _repository.Update(entityDb);
            }
            else
                await _repository.DeleteAsync(id);

            var sucesso = await _unitOfWork.CommitAsync();

            if (!sucesso)
                _notifiable.AddNotification("Deletar", $"Erro ao excluir o registro {id}");

            await Task.CompletedTask;
        }

        protected async Task Validate(TEntity item)
        {
            var method = item.GetType().GetMethod("Validar");

            if (method != null)
            {
                var erros = method.Invoke(item, null);
                _notifiable.AddNotifications(erros as IEnumerable<string>);
            }

            await Task.CompletedTask;
        }

    }
}
