namespace BookRec.Recommender
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BookRec.Infrastructure.EntityFramework.Extensions;
    using BookRec.Infrastructure.EntityFramework.Models;
    using BookRec.Infrastructure.EntityFramework.Repositories;
    using EnsureThat;
    using Microsoft.EntityFrameworkCore;

    public class CollaborativeRecommender : ICollaborativeRecommender
    {
        private readonly IUserBookRepository repository;

        public CollaborativeRecommender(IUserBookRepository repository)
        {
            EnsureArg.IsNotNull(repository);

            this.repository = repository;
        }

        public async Task<List<PredictionModel>> GetPredicationsByBooksAsync(List<UserBook> inputs, string username)
        {
            var helper = new CollaborativeRecommenderHelper(inputs);

            var userGroups = await (from userBook in this.repository.DbContext.UserBooks.Include(x => x.Book)
                                    join book in this.repository.DbContext.Books on userBook.BookId equals book.Id
                                    where userBook.Username != username
                                    group userBook by userBook.Username into userGroup
                                    select new
                                    {
                                                Group = userGroup,
                                                Temperature = helper.CalculateUTM(userGroup)
                                    })
                             .OrderByDescending(x => x.Temperature).Take(500).ToSafeListAsync();

            var outputWeights = userGroups.SelectMany(group =>
            {
                return group.Group.Where(x => !helper.Inputs.ContainsKey(x.BookId))
                        .Select(item => new
                        {
                            item.Book,
                            Score = helper.CalculateOPW(group.Temperature, item.Rating),
                            group.Temperature
                        });
            });

            return outputWeights.GroupBy(x => x.Book.Id)
                    .Select(group =>
                    {
                        var prediction = new PredictionModel()
                        {
                            Book = group.FirstOrDefault().Book
                        };

                        if (group.Count() == 1)
                        {
                            prediction.Score = group.FirstOrDefault().Score;
                        }
                        else
                        {
                            var maxWeight = group.Max(x => x.Score);
                            var imv = (1 - maxWeight) / group.Count();
                            prediction.Score = (double)maxWeight + group.Sum(x => imv * x.Temperature);
                        }

                        return prediction;
                    }).OrderByDescending(x => x.Score).Take(10).ToList();
        }
    }
}
