using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ShopApi.Entities;

namespace ShopApi.DataAccess
{
    public class ApplicationDbContext : IdentityDbContext<AppUser,AppRole,int,
                                IdentityUserClaim<int>,AppUserRole,IdentityUserLogin<int>,
                                IdentityRoleClaim<int>,IdentityUserToken<int>>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            

        }
        public DbSet<Product> Product { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            //g

        }

    }
}
