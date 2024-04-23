namespace BookRec.Infrastructure.EntityFramework.Context
{
    using BookRec.Infrastructure.EntityFramework.Models;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;

    public partial class BookRecContext : IdentityDbContext
    {
        private readonly string schema = "BookRec";

        public BookRecContext()
        {
        }

        public BookRecContext(DbContextOptions<BookRecContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Book> Books { get; set; }

        public virtual DbSet<UserBook> UserBooks { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(this.schema);

            // Model Configuration
            modelBuilder
                .BuildBookAggregateConfiguration()
                .BuildUserBookAggregateConfiguration()
                .BuildIdentityConfiguration();

            // Seeds
            //modelBuilder
            //    .BookAggregateSeeds();
            base.OnModelCreating(modelBuilder);
        }
    }
}
