using Estoque.Core.Entities;
using Estoque.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Estoque.Infra.Data
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : EntityBase
    {
        public readonly Context _context;

        public Repository(Context context)
        {
            _context = context;
        }

        public DbSet<TEntity> Table => _context.Set<TEntity>();

        public virtual async Task<IQueryable<TEntity>> GetAsync()
        {
            return await Task.FromResult(Table);
        }

        public virtual async Task<TEntity> GetAsync(Guid id)
        {
            return await Table.FirstOrDefaultAsync(x => x.Id == id);
        }

        public virtual async Task InsertAsync(TEntity entity)
        {
            await Table.AddAsync(entity);
        }


        public virtual async Task InsertAsync(List<TEntity> entities)
        {
            await Table.AddRangeAsync(entities);
        }

        public virtual void Update(TEntity entity)
        {
            Table.Update(entity);
        }

        public virtual void Update(IEnumerable<TEntity> entity)
        {
            Table.UpdateRange(entity);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            var entity = await GetAsync(id);
            Table.Remove(entity);
        }
    }
}
