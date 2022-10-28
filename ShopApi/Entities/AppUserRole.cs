using Microsoft.AspNetCore.Identity;

namespace ShopApi.Entities
{
    public class AppUserRole:IdentityUserRole<int>
    {
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
    //https://www.entityframeworktutorial.net/efcore/configure-many-to-many-relationship-in-ef-core.aspx
}
