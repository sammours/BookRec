namespace BookRec.Infrastructure.EntityFramework.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using BookRec.Recommender;

    public class PredictionModel
    {
        public Book Book { get; set; }

        public double Score { get; set; }
    }
}
