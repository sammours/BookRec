namespace BookRec.Infrastructure.EntityFramework.Context
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public static partial class Extensions
    {
        public static ModelBuilder BuildBookAggregateConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>(entity =>
            {
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Title).IsRequired();
                entity.HasMany(e => e.BookUsers);
            });

            return modelBuilder;
        }
    }
}
