namespace BookRec.Infrastructure.EntityFramework.Context
{
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public static partial class Extensions
    {
        public static ModelBuilder BuildIdentityConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(e => e.LoginProvider);

            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasKey(e => new { e.UserId, e.RoleId });

            modelBuilder.Entity<IdentityUserToken<string>>()
                .HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

            return modelBuilder;
        }
    }
}
