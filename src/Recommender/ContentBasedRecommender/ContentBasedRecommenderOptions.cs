namespace BookRec.Recommender
{
    using System.Collections.Generic;
    using System.Linq;
    using BookRec.Common;
    using BookRec.Infrastructure.EntityFramework.Models;
    using EnsureThat;

    public class ContentBasedRecommenderOptions
    {
        public ContentBasedRecommenderOptions(List<Book> inputs)
        {
            EnsureArg.IsNotNull(inputs);

            this.FilterCategories(inputs);
            this.FilterMaturityRating(inputs);
            this.FilterLanguageCode(inputs);
            this.FilterCountry(inputs);
            this.FilterAuthors(inputs);
            this.FilterPublisher(inputs);
            this.FilterPublishedDate(inputs);
        }

        public List<string> Categories { get; set; }

        public List<string> MaturityRating { get; set; }

        public List<string> LanguageCode { get; set; }

        public List<string> Country { get; set; }

        public List<string> Authors { get; set; }

        public List<string> Publisher { get; set; }

        public List<int> PublishedYear { get; set; }

        public double HotFactorsSatisfaction(Book book)
            => (this.Categories.Any(x => x == book.Categories) ? 1 : 0) +
               (this.LanguageCode.Any(x => x == book.LanguageCode) ? 1 : 0) +
               (this.Country.Any(x => x == book.Country) ? 1 : 0) +
               (this.MaturityRating.Any(x => x == book.MaturityRating) ? 1 : 0);

        public double WarmFactorsSatisfaction(Book book)
            => (this.Authors.Any(x => x == book.Authors) ? 0.5 : 0) +
                (this.Publisher.Any(x => x == book.Publisher) ? 0.5 : 0) +
                (this.PublishedYear.Any(x => book.PublishedDate.ToNearistCentury() == x) ? 0.5 : 0);

        public double MinimumWeight() => 4;

        public double TotalWeight() => 4;

        public double CalculateScore(double weight) => weight / 5.5;

        private void FilterCategories(List<Book> inputs)
        {
            var groups = inputs.GroupBy(x => x.Categories);
            var threshold = (double)groups.Sum(group => group.Count()) / groups.Count();
            this.Categories = groups.Where(group => group.Count() >= threshold)
                                    .SelectMany(group => group.Select(x => x.Categories))
                                    .Where(x => x != null).Distinct().ToList();
        }

        private void FilterMaturityRating(List<Book> inputs)
        {
            var groups = inputs.GroupBy(x => x.MaturityRating);
            var threshold = (double)groups.Sum(group => group.Count()) / groups.Count();
            this.MaturityRating = groups.Where(group => group.Count() >= threshold).SelectMany(group => group.Select(x => x.MaturityRating)).Where(x => x != null).Distinct().ToList();
        }

        private void FilterLanguageCode(List<Book> inputs)
        {
            var groups = inputs.GroupBy(x => x.LanguageCode);
            var threshold = (double)groups.Sum(group => group.Count()) / groups.Count();
            this.LanguageCode = groups.Where(group => group.Count() >= threshold).SelectMany(group => group.Select(x => x.LanguageCode)).Where(x => x != null).Distinct().ToList();
        }

        private void FilterCountry(List<Book> inputs)
        {
            var groups = inputs.GroupBy(x => x.Country);
            var threshold = (double)groups.Sum(group => group.Count()) / groups.Count();
            this.Country = groups.Where(group => group.Count() >= threshold).SelectMany(group => group.Select(x => x.Country)).Where(x => x != null).Distinct().ToList();
        }

        private void FilterAuthors(List<Book> inputs)
        {
            var groups = inputs.GroupBy(x => x.Authors);
            var threshold = (double)groups.Sum(group => group.Count()) / groups.Count();
            this.Authors = groups.Where(group => group.Count() >= threshold).SelectMany(group => group.Select(x => x.Authors)).Where(x => x != null).Distinct().ToList();
        }

        private void FilterPublisher(List<Book> inputs)
        {
            var groups = inputs.GroupBy(x => x.Country);
            var threshold = (double)groups.Sum(group => group.Count()) / groups.Count();
            this.Publisher = groups.Where(group => group.Count() >= threshold).SelectMany(group => group.Select(x => x.Publisher)).Where(x => x != null).Distinct().ToList();
        }

        private void FilterPublishedDate(List<Book> inputs)
        {
            var groups = inputs.GroupBy(x => x.PublishedDate.ToNearistCentury());
            var threshold = (double)groups.Sum(group => group.Count()) / groups.Count();
            this.PublishedYear = groups.Where(group => group.Count() >= threshold).SelectMany(group => group.Where(x => x != null).Select(x => x.PublishedDate.ToNearistCentury())).Distinct().ToList();
        }
    }
}
