using Estoque.Core.Entities;

namespace Estoque.Core.Interfaces
{
    public interface IService<TEntity> where TEntity : EntityBase
    {
        Task<IEnumerable<TEntity>> GetAsync();
        Task<TEntity> GetAsync(Guid id);
        Task<Guid> InsertAsync(TEntity entity);
        Task UpdateAsync(Guid id, TEntity entity);
        Task UpdateAsync(List<TEntity> entities);
        Task DeleteAsync(Guid id);

    }
}
