using Microsoft.AspNetCore.Identity;

namespace ShopApi.Entities
{
    public class AppUser:IdentityUser<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }

    }
}
