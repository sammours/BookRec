namespace BookRec.Recommender
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BookRec.Infrastructure.EntityFramework.Models;
    using EnsureThat;

    public class CollaborativeRecommenderHelper
    {
        public CollaborativeRecommenderHelper(List<UserBook> inputs)
        {
            EnsureArg.IsNotNull(inputs);

            this.CalculateTotalRating(inputs);
            this.SetInputs(inputs);
        }

        /// <summary>
        /// Number of Inputs
        /// </summary>
        public int TotalRating { get; set; }

        /// <summary>
        /// Total Temperature
        /// </summary>
        public int TTM { get; set; }

        /// <summary>
        /// Inputs
        /// </summary>
        public Dictionary<Guid, int> Inputs { get; set; } = new Dictionary<Guid, int>();

        /// <summary>
        /// Caclculate User Temperature (UTM)
        /// </summary>
        /// <param name="userGroup">All books read by the user</param>
        /// <returns>UTM</returns>
        public double CalculateUTM(IGrouping<string, UserBook> userGroup)
            => (double)1 - this.Inputs.Select(input =>
            {
                var book = userGroup.FirstOrDefault(x => x.BookId == input.Key);
                return book == null ? input.Value : input.Value - book.Rating;
            }).Sum(value => (double)value / this.TotalRating);

        /// <summary>
        /// Calculate the output weight of a prediction (OPW)
        /// </summary>
        /// <param name="temperature">Reader temperature</param>
        /// <param name="rating">Reader rating</param>
        /// <returns>OPW</returns>
        public double CalculateOPW(double temperature, int rating)
            => temperature * rating / 5;

        /// <summary>
        /// Calculate the number of inputs (NOI)
        /// </summary>
        /// <param name="inputs">Input set</param>
        private void CalculateTotalRating(List<UserBook> inputs)
            => this.TotalRating = inputs.Sum(x => x.Rating);

        /// <summary>
        /// Set input ids
        /// </summary>
        /// <param name="inputs">Input set</param>
        private void SetInputs(List<UserBook> inputs)
            => inputs.ForEach(x => this.Inputs.Add(x.BookId, x.Rating));
    }
}
