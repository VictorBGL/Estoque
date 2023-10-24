using Estoque.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Estoque.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : EntityBase
    {
        public DbSet<TEntity> Table { get; }
        Task<IQueryable<TEntity>> GetAsync();
        Task<TEntity> GetAsync(Guid id);
        Task InsertAsync(TEntity entity);
        Task InsertAsync(List<TEntity> entities);
        void Update(TEntity entity);
        void Update(IEnumerable<TEntity> entity);
        //void Upsert(TEntity entity);
        Task DeleteAsync(Guid id);
    }
}
