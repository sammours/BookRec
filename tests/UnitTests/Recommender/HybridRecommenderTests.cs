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

    public class HybridRecommenderTests
    {
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
                var cbfRecommender = new ContentBasedRecommender(new BookRepository(context));
                var cfRecommender = new CollaborativeRecommender(new UserBookRepository(context));
                var sub = new HybridRecommender(cbfRecommender, cfRecommender);

                var prediction = await sub.GetPredicationsByBooksAsync(inputs, "InputUser").ConfigureAwait(false);

                // assert
                Assert.NotNull(prediction);
                Assert.Equal(2, prediction.Count());
                Assert.Equal(prediction.Max(x => x.Score), prediction.FirstOrDefault().Score);
                Assert.Equal("1920ca07-3054-496a-8e9f-01d98bc71165".ToGuid(), prediction.FirstOrDefault().Book.Id);
            }
        }

        private List<UserBook> GetInputs() => this.InitialUserData().Where(x => x.Username == "InputUser").ToList();

        private List<Book> InitialBookData()
            => new List<Book>
            {
                new Book()
                {
                    Id = "2bde613c-7f2b-449b-8c1e-0511f9aa5702".ToGuid().Value,
                    Title = "Art in museums",
                    Authors = "Grace Morley",
                    Publisher = "Germany",
                    PublishedDate = new DateTime(1988, 1, 1),
                    Categories = "Art",
                    MaturityRating = "NOT_MATURE",
                    LanguageCode = "en",
                    Country = "de"
                },
                new Book()
                {
                    Id = "b75de15d-8fc3-400a-dd98-08d6ff11e8e2".ToGuid().Value,
                    Title = "Arts of Power",
                    Authors = "Randolph Starn",
                    Publisher = "Univ of California Press",
                    PublishedDate = new DateTime(1992, 1, 1),
                    Categories = "Art",
                    MaturityRating = "NOT_MATURE",
                    LanguageCode = "en",
                    Country = "de"
                },
                new Book()
                {
                    Id = "c2a48df5-ef5b-4752-8352-00326a1b60ac".ToGuid().Value,
                Title = "What Is to Be Done?",
                    Authors = "Nikolai Chernyshevsky",
                    Publisher = "Cornell University Press",
                    PublishedDate = new DateTime(2014, 1, 1),
                    Categories = "Fiction",
                    MaturityRating = "NOT_MATURE",
                    LanguageCode = "en",
                    Country = "de"
                },
                new Book()
                {
                    Id = "949D121A-8ED2-4C20-B234-00FADF5A8297".ToGuid().Value,
                    Title = "Murder by Ice",
                    Authors = "Lorraine Burrell Hughes",
                    Publisher = "Xlibris Corporation",
                    PublishedDate = new DateTime(2012, 1, 1),
                    Categories = "Fiction",
                    MaturityRating = "NOT_MATURE",
                    LanguageCode = "en",
                    Country = "de"
                },
                new Book()
                {
                    Id = "1920CA07-3054-496A-8E9F-01D98BC71165".ToGuid().Value,
                    Title = "The Arts of Korea: Kim, W. Y., Choi, S. U., Im, C. S. Paintings",
                    Authors = "Tonghwa Chʻulpʻansa",
                    Publisher = "Xlibris Corporation",
                    PublishedDate = new DateTime(2002, 1, 1),
                    Categories = "Art",
                    MaturityRating = "NOT_MATURE",
                    LanguageCode = "en",
                    Country = "de"
                }
            };

        private List<UserBook> InitialUserData()
            => new List<UserBook>()
            {
                new UserBook()
                {
                    Id = "ba4cacac-3dda-4af3-c3cb-08d715cd23c8".ToGuid().Value,
                    Username = "InputUser",
                    Rating = 4,
                    BookId="2bde613c-7f2b-449b-8c1e-0511f9aa5702".ToGuid().Value,
                    Book = this.InitialBookData().FirstOrDefault(x => x.Id == "2bde613c-7f2b-449b-8c1e-0511f9aa5702".ToGuid().Value)
                },
                new UserBook()
                {
                    Id = "5196d81e-d8c4-4cc8-c3cc-08d715cd23c8".ToGuid().Value,
                    Username = "InputUser",
                    Rating = 3,
                    BookId="b75de15d-8fc3-400a-dd98-08d6ff11e8e2".ToGuid().Value,
                    Book = this.InitialBookData().FirstOrDefault(x => x.Id == "b75de15d-8fc3-400a-dd98-08d6ff11e8e2".ToGuid().Value)
                },
                new UserBook()
                {
                    Id = "1ec69a48-f8f9-4b33-c3cd-08d715cd23c8".ToGuid().Value,
                    Username = "InputUser",
                    Rating = 4,
                    BookId="c2a48df5-ef5b-4752-8352-00326a1b60ac".ToGuid().Value,
                    Book = this.InitialBookData().FirstOrDefault(x => x.Id == "c2a48df5-ef5b-4752-8352-00326a1b60ac".ToGuid().Value)
                },

                new UserBook()
                {
                    Id = "DFEBCF9C-83C4-42C1-DC89-08D71675B384".ToGuid().Value,
                    Username = "User2",
                    Rating = 4,
                    BookId="2BDE613C-7F2B-449B-8C1E-0511F9AA5702".ToGuid().Value,
                    Book = this.InitialBookData().FirstOrDefault(x => x.Id == "2BDE613C-7F2B-449B-8C1E-0511F9AA5702".ToGuid().Value)
                },
                new UserBook()
                {
                    Id = "904FA94B-11CE-43E7-DC8A-08D71675B384".ToGuid().Value,
                    Username = "User2",
                    Rating = 4,
                    BookId="949D121A-8ED2-4C20-B234-00FADF5A8297".ToGuid().Value,
                    Book = this.InitialBookData().FirstOrDefault(x => x.Id == "949D121A-8ED2-4C20-B234-00FADF5A8297".ToGuid().Value)
                },

                new UserBook()
                {
                    Id = "66529B58-CE98-4E20-DC8D-08D71675B384".ToGuid().Value,
                    Username = "User3",
                    Rating = 4,
                    BookId="B75DE15D-8FC3-400A-DD98-08D6FF11E8E2".ToGuid().Value,
                    Book = this.InitialBookData().FirstOrDefault(x => x.Id == "B75DE15D-8FC3-400A-DD98-08D6FF11E8E2".ToGuid().Value)
                },
                new UserBook()
                {
                    Id = "6225A480-84F0-45CB-DC8E-08D71675B384".ToGuid().Value,
                    Username = "User3",
                    Rating = 2,
                    BookId="949D121A-8ED2-4C20-B234-00FADF5A8297".ToGuid().Value,
                    Book = this.InitialBookData().FirstOrDefault(x => x.Id == "949D121A-8ED2-4C20-B234-00FADF5A8297".ToGuid().Value)
                },

                new UserBook()
                {
                    Id = "3476BEC3-A51E-4699-DC92-08D71675B384".ToGuid().Value,
                    Username = "User4",
                    Rating = 3,
                    BookId="1920CA07-3054-496A-8E9F-01D98BC71165".ToGuid().Value,
                    Book = this.InitialBookData().FirstOrDefault(x => x.Id == "1920CA07-3054-496A-8E9F-01D98BC71165".ToGuid().Value)
                },
                new UserBook()
                {
                    Id = "4A2D96C1-473E-4AE3-DC93-08D71675B384".ToGuid().Value,
                    Username = "User4",
                    Rating = 4,
                    BookId="C2A48DF5-EF5B-4752-8352-00326A1B60AC".ToGuid().Value,
                    Book = this.InitialBookData().FirstOrDefault(x => x.Id == "C2A48DF5-EF5B-4752-8352-00326A1B60AC".ToGuid().Value)
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
            context.SaveChanges();
        }
    }
}