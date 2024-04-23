namespace BookRec.Tests.UnitTests
{
    using BookRec.Infrastructure.EntityFramework.Models;
    using Microsoft.EntityFrameworkCore;

    public class StubDbContext : DbContext
    {
        public StubDbContext()
        {
        }

        public StubDbContext(DbContextOptions<StubDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Entities { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
