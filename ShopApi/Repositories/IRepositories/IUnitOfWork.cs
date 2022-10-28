namespace ShopApi.Repositories.IRepositories
{
    public interface IUnitOfWork:IDisposable
    {
        IProductRepository ProductRepository { get; }
        Task<bool> Complete();
        bool HasChanges();
    }
}
