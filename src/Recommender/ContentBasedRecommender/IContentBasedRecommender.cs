namespace BookRec.Recommender
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BookRec.Infrastructure.EntityFramework.Models;

    public interface IContentBasedRecommender
    {
        Task<List<PredictionModel>> GetPredicationsByBooksAsync(List<Book> inputs);
    }
}
