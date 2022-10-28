using ShopApi.Entities;

namespace ShopApi.Services
{
    public interface IJwtTokenService
    {
        //Task<Tuple<string, DateTime>> GenerateJwt(AppUser User);
        Task<string> CreateToken(AppUser user);

    }
}
