namespace BookRec.Recommender
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BookRec.Infrastructure.EntityFramework.Models;

    public interface ICollaborativeRecommender
    {
        Task<List<PredictionModel>> GetPredicationsByBooksAsync(List<UserBook> inputs, string username);
    }
}
