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

    public class ContentBasedRecommenderTests
    {
        [Fact]
        public void InitOption_Options_Test()
        {
            // arrange
            var inputs = this.InitialData();

            // act
            var options = new ContentBasedRecommenderOptions(inputs);
            inputs.ForEach(x =>
            {
                if (x.Id == "C2A48DF5-EF5B-4752-8352-00326A1B60AC".ToGuid().Value || x.Id == "949D121A-8ED2-4C20-B234-00FADF5A8297".ToGuid().Value)
                {
                    x.LanguageCode = "de";
                }
            });
            var options2 = new ContentBasedRecommenderOptions(inputs);

            // assert
            Assert.NotNull(options.Categories);
            Assert.True(options.Categories.Count() == 2);

            Assert.NotNull(options.LanguageCode);
            Assert.True(options.LanguageCode.Count() == 1);

            Assert.NotNull(options.Country);
            Assert.True(options.Country.Count() == 1);

            Assert.NotNull(options2.LanguageCode);
            Assert.True(options2.LanguageCode.Count() == 2);
        }

        [Fact]
        public void FactorSatisfaction_Options_Test()
        {
            // arrange
            var inputs = this.InitialData();

            // act
            var options = new ContentBasedRecommenderOptions(inputs);
            var book1 = new Book()
            {
                Authors = "Tonghwa Chʻulpʻansa",
                Publisher = "Germany",
                PublishedDate = new DateTime(2001, 1, 1),
                Categories = "Art",
                MaturityRating = "NOT_MATURE",
                LanguageCode = "en",
                Country = "de"
            };

            var book2 = new Book()
            {
                Authors = "Tonghwa Chʻulpʻansa",
                Publisher = "Franch",
                PublishedDate = new DateTime(2001, 1, 1),
                Categories = "History",
                MaturityRating = "MATURE",
                LanguageCode = "fr",
                Country = "de"
            };

            // assert
            Assert.True(options.HotFactorsSatisfaction(book1) == 4);
            Assert.True(options.WarmFactorsSatisfaction(book1) == 1.5);

            Assert.True(options.HotFactorsSatisfaction(book2) == 1);
            Assert.True(options.WarmFactorsSatisfaction(book2) == 1);
        }

        [Fact]
        public void ScoreCalculation_Options_Test()
        {
            // arrange
            var inputs = this.InitialData();

            // act
            var options = new ContentBasedRecommenderOptions(inputs);

            // assert
            Assert.True(Math.Round(options.CalculateScore(5.5), 3) == 1);
            Assert.True(Math.Round(options.CalculateScore(4.5), 3) == 0.818);
            Assert.True(Math.Round(options.CalculateScore(1.5), 3) == 0.273);
        }

        [Fact]
        public async Task GetPredictions_Recommender_Test()
        {
            using (var context = new BookRecContext(this.DbOptions()))
            {
                // arrange
                this.SeedData(context);
                var repository = new BookRepository(context);
                var sub = new ContentBasedRecommender(repository);

                // act
                var book = new Book()
                {
                    Id = "1920CA07-3054-496A-8E9F-01D98BC71165".ToGuid().Value,
                    Authors = "Tonghwa Chʻulpʻansa",
                    Publisher = "Franch",
                    PublishedDate = new DateTime(0001, 1, 1),
                    Categories = "Art",
                    MaturityRating = "NOT_MATURE",
                    LanguageCode = "en",
                    Country = "de"
                };
                var inputs = new List<Book>() { book };
                var options = new ContentBasedRecommenderOptions(inputs);
                var result = await sub.GetPredicationsByBooksAsync(inputs).ConfigureAwait(false);

                book.MaturityRating = "MATURE";
                var result2 = await sub.GetPredicationsByBooksAsync(inputs).ConfigureAwait(false);

                book.MaturityRating = "NOT_MATURE";
                inputs.Add(new Book()
                {
                    Id = Guid.NewGuid(),
                    Title = "What Is to Be Done?",
                    Authors = "Nikolai Chernyshevsky",
                    Publisher = "Cornell University Press",
                    PublishedDate = new DateTime(2015, 5, 29),
                    Categories = "Fiction",
                    MaturityRating = "NOT_MATURE",
                    LanguageCode = "en",
                    Country = "de"
                });
                var result3 = await sub.GetPredicationsByBooksAsync(inputs).ConfigureAwait(false);

                // assert
                Assert.True(result.Count() == 2);
                Assert.True(result.FirstOrDefault().Score == options.CalculateScore(4.5));

                Assert.True(result2.Count() == 0);

                Assert.True(result3.Count() == 4);
                Assert.True(result3.FirstOrDefault().Score == options.CalculateScore(5.5));
            }
        }

        private List<Book> InitialData()
            => new List<Book>()
            {
                new Book()
                {
                    Id = "1920CA07-3054-496A-8E9F-01D98BC71165".ToGuid().Value,
                    Title = "The Arts of Korea: Kim, W.Y., Choi, S.U., Im, C.S.Paintings",
                    Authors = "Tonghwa Chʻulpʻansa",
                    Publisher = "Germany",
                    PublishedDate = new DateTime(0001, 1, 1),
                    Categories = "Art",
                    MaturityRating = "NOT_MATURE",
                    LanguageCode = "en",
                    Country = "de"
                },
                new Book()
                {
                    Id = "2BDE613C-7F2B-449B-8C1E-0511F9AA5702".ToGuid().Value,
                    Title = "Art in museums lectures",
                    Authors = "Grace Morley",
                    Publisher = "Germany",
                    PublishedDate = new DateTime(0001, 1, 1),
                    Categories = "Art",
                    MaturityRating = "NOT_MATURE",
                    LanguageCode = "en",
                    Country = "de"
                },
                new Book()
                {
                    Id = "B75DE15D-8FC3-400A-DD98-08D6FF11E8E2".ToGuid().Value,
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
                    Id = "C2A48DF5-EF5B-4752-8352-00326A1B60AC".ToGuid().Value,
                    Title = "What Is to Be Done?",
                    Authors = "Nikolai Chernyshevsky",
                    Publisher = "Cornell University Press",
                    PublishedDate = new DateTime(2015, 5, 29),
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
                    PublishedDate = new DateTime(2012, 2, 1),
                    Categories = "Fiction",
                    MaturityRating = "NOT_MATURE",
                    LanguageCode = "en",
                    Country = "de"
                },
                new Book()
                {
                    Id = "C3C38179-5AFC-4F7B-AB8A-0190D2CCF3DB".ToGuid().Value,
                    Title = "Beautiful Disaster",
                    Authors = "Jamie McGuire",
                    Publisher = "Piper Verlag",
                    PublishedDate = new DateTime(2013, 4, 16),
                    Categories = "Fiction",
                    MaturityRating = "MATURE",
                    LanguageCode = "de",
                    Country = "de"
                }
            };

        private DbContextOptions<BookRecContext> DbOptions()
            => new DbContextOptionsBuilder<BookRecContext>()
                .UseInMemoryDatabase(databaseName: "test")
                .Options;

        private void SeedData(BookRecContext context)
        {
            context.Books.RemoveRange(context.Books);

            var entities = this.InitialData();

            context.Books.AddRange(entities);
            context.SaveChanges();
        }
    }
}