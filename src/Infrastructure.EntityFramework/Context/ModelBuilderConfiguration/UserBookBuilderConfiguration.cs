namespace BookRec.Infrastructure.EntityFramework.Context
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public static partial class Extensions
    {
        public static ModelBuilder BuildUserBookAggregateConfiguration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserBook>(entity =>
            {
                entity.Property(e => e.Id).IsRequired();
                entity.Property(e => e.Username).IsRequired();
                entity.Property(e => e.BookId).IsRequired();
                entity.HasIndex(e => e.Username);
                entity.HasIndex(e => e.BookId);
                entity.HasOne(e => e.Book);
                entity.ToTable("UserBooks");
            });

            return modelBuilder;
        }
    }
}
