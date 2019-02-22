using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using User.System.Core.Repository.Core.Interfaces;

namespace User.System.Core.Repository.Core
{
    public class RepositoryAsync<TEntity> : IRepositoryAsync<TEntity> where TEntity : class
    {
        protected readonly SocketDbContext Context;

        public RepositoryAsync(SocketDbContext context)
        {
            Context = context;
        }
        public async Task<TEntity> GetAsync(int id)
        {
            return await Context.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await Task.Run(() => Context.Set<TEntity>().ToList());
        }

        public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.Run(() => Context.Set<TEntity>().Where(predicate));
        }

        public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await Task.Run(() => Context.Set<TEntity>().FirstOrDefault(predicate));
        }

        public async Task AddAsync(TEntity entity)
        {
            await Task.Run(() => Context.Set<TEntity>().Add(entity));
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => Context.Set<TEntity>().AddRange(entities));
        }

        public async Task RemoveAsync(TEntity entity)
        {
            await Task.Run(() => Context.Set<TEntity>().Remove(entity));
        }

        public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            await Task.Run(() => Context.Set<TEntity>().RemoveRange(entities));
        }
    }
}
