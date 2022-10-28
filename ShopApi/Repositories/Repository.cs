
using Microsoft.EntityFrameworkCore;
using ShopApi.DataAccess;
using ShopApi.Repositories.IRepositories;
using System.Linq.Expressions;

namespace ShopApi.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public Repository(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public void Add(TEntity entity)
        {
            _applicationDbContext.Set<TEntity>().Add(entity);
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            _applicationDbContext.Set<TEntity>().AddRange(entities);
        }

        public async Task<IEnumerable<TEntity>> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return await _applicationDbContext.Set<TEntity>().Where(predicate).ToListAsync();
        }

        public async Task<TEntity> FindOne(int id)
        {
            return await _applicationDbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<TEntity> Get(int id)
        {
            return await _applicationDbContext.Set<TEntity>().FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAll()
        {
            return await _applicationDbContext.Set<TEntity>().ToListAsync();
        }

        public void Remove(TEntity entity)
        {
            _applicationDbContext.Set<TEntity>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            _applicationDbContext.Set<TEntity>().RemoveRange(entities);
        }

        public virtual void Update(TEntity entity)
        {
            _applicationDbContext.Entry(entity).State = EntityState.Modified;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            _applicationDbContext.Set<TEntity>().UpdateRange(entities);
        }
    }
}
