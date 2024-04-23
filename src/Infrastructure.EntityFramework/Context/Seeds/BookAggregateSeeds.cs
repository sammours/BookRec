namespace BookRec.Infrastructure.EntityFramework.Context
{
    using Microsoft.EntityFrameworkCore;

    public static partial class Extensions
    {
        public static ModelBuilder BookAggregateSeeds(this ModelBuilder modelBuilder)
        {
            //var questions = new List<Book>();
            //for (int i = 1; i <= 100; i++)
            //{
            //    var book = new Book()
            //    {
            //        Id = i
            //    };

            //    questions.Add(book);
            //}

            //modelBuilder.Entity<Book>().HasData(questions.ToArray());
            return modelBuilder;
        }
    }
}
