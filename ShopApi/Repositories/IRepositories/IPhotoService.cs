using CloudinaryDotNet.Actions;

namespace ShopApi.Repositories.IRepositories
{
    public interface IPhotoService
    {
        Task<ImageUploadResult> AddPhotoAsync(IFormFile file);
        Task<DeletionResult> DeletePhotoAsync(string publicID); 
    }
}
