using ShopApi.DataAccess;
using ShopApi.Entities;
using ShopApi.Repositories.IRepositories;

namespace ShopApi.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
        }
        //https://www.youtube.com/watch?v=rtXpYpZdOzM&ab_channel=ProgrammingwithMosh
    }
}
