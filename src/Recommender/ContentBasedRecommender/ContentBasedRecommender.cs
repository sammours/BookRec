namespace BookRec.Recommender
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BookRec.Common;
    using BookRec.Infrastructure.EntityFramework.Extensions;
    using BookRec.Infrastructure.EntityFramework.Models;
    using BookRec.Infrastructure.EntityFramework.Repositories;
    using EnsureThat;

    public class ContentBasedRecommender : IContentBasedRecommender
    {
        private readonly IBookRepository repository;

        public ContentBasedRecommender(IBookRepository repository)
        {
            EnsureArg.IsNotNull(repository);

            this.repository = repository;
        }

        public async Task<List<PredictionModel>> GetPredicationsByBooksAsync(List<Book> inputs)
        {
            if (inputs == null || !inputs.Any())
            {
                return new List<PredictionModel>();
            }

            var options = new ContentBasedRecommenderOptions(inputs);
            return await (from book in this.repository.DbContext.Books
                          let weight = options.HotFactorsSatisfaction(book) +
                                       options.WarmFactorsSatisfaction(book)
                          where weight >= options.MinimumWeight()
                          where inputs.All(x => x.Id != book.Id)
                          select new PredictionModel
                          {
                              Book = book,
                              Score = options.CalculateScore(weight)
                          }).OrderByDescending(x => x.Score).Take(10).ToSafeListAsync();
        }
    }
}
