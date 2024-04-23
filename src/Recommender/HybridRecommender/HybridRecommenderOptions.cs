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

    public class HybridRecommenderOptions
    {
        public HybridRecommenderOptions()
        {
        }

        public double CBFWeight { get; set; } = 0.5;

        public double CFWeight { get; set; } = 0.5;

        public double CBFScore(double score) => score * this.CBFWeight;

        public double CFScore(double score) => score * this.CFWeight;
    }
}
