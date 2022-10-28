using ShopApi.DataAccess;
using ShopApi.Repositories.IRepositories;

namespace ShopApi.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public UnitOfWork(ApplicationDbContext applicationDbContext)
        {
            this._applicationDbContext = applicationDbContext;
        }

        public IProductRepository ProductRepository => new ProductRepository(_applicationDbContext);

        public async Task<bool> Complete()
        {
            return (await _applicationDbContext.SaveChangesAsync() > 0);
        }
        public bool HasChanges()
        {
            _applicationDbContext.ChangeTracker.DetectChanges();
            var changes = _applicationDbContext.ChangeTracker.HasChanges();

            return changes;
        }
        public void Dispose()
        {
            _applicationDbContext.Dispose();
        }

    }
}
