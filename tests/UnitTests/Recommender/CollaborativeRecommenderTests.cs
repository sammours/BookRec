namespace BookRec.Tests.UnitTests.Recommender
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BookRec.Infrastructure.EntityFramework.Context;
    using BookRec.Infrastructure.EntityFramework.Models;
    using BookRec.Infrastructure.EntityFramework.Repositories;
    using BookRec.Recommender;
    using Common;
    using Microsoft.EntityFrameworkCore;
    using Xunit;

    public class CollaborativeRecommenderTests
    {
        [Fact]
        public void CalculateTotalRating_Options_Test()
        {
            // arrange
            var inputs = this.GetInputs();

            // act
            var options = new CollaborativeRecommenderHelper(inputs);

            // assert
            Assert.True(options.TotalRating == 11);
        }

        [Fact]
        public void CalculateUserTemperature_Options_Test()
        {
            // arrange
            var inputs = this.GetInputs();

            // act
            var options = new CollaborativeRecommenderHelper(inputs);
            var group = this.GetFirstUser().GroupBy(x => x.Username).FirstOrDefault();
            var temperature = options.CalculateUTM(group);

            // assert
            Assert.True(temperature == 0.36363636363636365);
        }

        [Fact]
        public void CalculateOPW_Options_Test()
        {
            // arrange
            var inputs = this.GetInputs();

            // act
            var options = new CollaborativeRecommenderHelper(inputs);
            var group = this.GetFirstUser().GroupBy(x => x.Username).FirstOrDefault();
            var opw = options.CalculateOPW(options.CalculateUTM(group), group.LastOrDefault().Rating);

            // assert
            Assert.True(opw == 0.29090909090909089);
        }

        [Fact]
        public async Task GetPredictions_Recommender_Test()
        {
            // arrange
            var inputs = this.GetInputs();

            // act
            using (var context = new BookRecContext(this.DbOptions()))
            {
                // arrange
                this.SeedData(context);
                var repository = new UserBookRepository(context);
                var sub = new CollaborativeRecommender(repository);

                var prediction = await sub.GetPredicationsByBooksAsync(inputs, "InputUser").ConfigureAwait(false);

                // assert
                Assert.NotNull(prediction);
                Assert.Equal(2, prediction.Count());
                Assert.Equal(prediction.Max(x => x.Score), prediction.FirstOrDefault().Score);
                Assert.Equal("949D121A-8ED2-4C20-B234-00FADF5A8297".ToGuid(), prediction.FirstOrDefault().Book.Id);
            }
        }

        private List<UserBook> GetInputs() => this.InitialUserData().Where(x => x.Username == "InputUser").ToList();

        private List<UserBook> GetFirstUser() => this.InitialUserData().Where(x => x.Username != "InputUser").GroupBy(x => x.Username).FirstOrDefault().Select(x => x).ToList();

        private List<Book> InitialBookData()
            => new List<Book>
            {
                new Book() { Id = "2bde613c-7f2b-449b-8c1e-0511f9aa5702".ToGuid().Value },
                new Book() { Id = "b75de15d-8fc3-400a-dd98-08d6ff11e8e2".ToGuid().Value },
                new Book() { Id = "c2a48df5-ef5b-4752-8352-00326a1b60ac".ToGuid().Value },
                new Book() { Id = "949D121A-8ED2-4C20-B234-00FADF5A8297".ToGuid().Value },
                new Book() { Id = "1920CA07-3054-496A-8E9F-01D98BC71165".ToGuid().Value }
            };

        private List<UserBook> InitialUserData()
            => new List<UserBook>()
            {
                new UserBook()
                {
                    Id = "ba4cacac-3dda-4af3-c3cb-08d715cd23c8".ToGuid().Value,
                    Username = "InputUser",
                    Rating = 4,
                    BookId="2bde613c-7f2b-449b-8c1e-0511f9aa5702".ToGuid().Value
                },
                new UserBook()
                {
                    Id = "5196d81e-d8c4-4cc8-c3cc-08d715cd23c8".ToGuid().Value,
                    Username = "InputUser",
                    Rating = 3,
                    BookId="b75de15d-8fc3-400a-dd98-08d6ff11e8e2".ToGuid().Value
                },
                new UserBook()
                {
                    Id = "1ec69a48-f8f9-4b33-c3cd-08d715cd23c8".ToGuid().Value,
                    Username = "InputUser",
                    Rating = 4,
                    BookId="c2a48df5-ef5b-4752-8352-00326a1b60ac".ToGuid().Value
                },

                new UserBook()
                {
                    Id = "DFEBCF9C-83C4-42C1-DC89-08D71675B384".ToGuid().Value,
                    Username = "User2",
                    Rating = 4,
                    BookId="2BDE613C-7F2B-449B-8C1E-0511F9AA5702".ToGuid().Value
                },
                new UserBook()
                {
                    Id = "904FA94B-11CE-43E7-DC8A-08D71675B384".ToGuid().Value,
                    Username = "User2",
                    Rating = 4,
                    BookId="949D121A-8ED2-4C20-B234-00FADF5A8297".ToGuid().Value
                },

                new UserBook()
                {
                    Id = "66529B58-CE98-4E20-DC8D-08D71675B384".ToGuid().Value,
                    Username = "User3",
                    Rating = 4,
                    BookId="B75DE15D-8FC3-400A-DD98-08D6FF11E8E2".ToGuid().Value
                },
                new UserBook()
                {
                    Id = "6225A480-84F0-45CB-DC8E-08D71675B384".ToGuid().Value,
                    Username = "User3",
                    Rating = 2,
                    BookId="949D121A-8ED2-4C20-B234-00FADF5A8297".ToGuid().Value
                },

                new UserBook()
                {
                    Id = "3476BEC3-A51E-4699-DC92-08D71675B384".ToGuid().Value,
                    Username = "User4",
                    Rating = 3,
                    BookId="1920CA07-3054-496A-8E9F-01D98BC71165".ToGuid().Value
                },
                new UserBook()
                {
                    Id = "4A2D96C1-473E-4AE3-DC93-08D71675B384".ToGuid().Value,
                    Username = "User4",
                    Rating = 4,
                    BookId="C2A48DF5-EF5B-4752-8352-00326A1B60AC".ToGuid().Value
                }
            };

        private DbContextOptions<BookRecContext> DbOptions()
            => new DbContextOptionsBuilder<BookRecContext>()
                .UseInMemoryDatabase(databaseName: "test")
                .Options;

        private void SeedData(BookRecContext context)
        {
            context.UserBooks.RemoveRange(context.UserBooks);
            context.Books.RemoveRange(context.Books);

            context.UserBooks.AddRange(this.InitialUserData());
            context.Books.AddRange(this.InitialBookData());
            context.SaveChanges();
        }
    }
}