namespace BookRec.App.Areas.Charts.Pages
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using BookRec.App.Model;
    using BookRec.Recommender;
    using EnsureThat;
    using Infrastructure.EntityFramework.Repositories;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IContentBasedRecommender contentBasedRecommender;
        private readonly ICollaborativeRecommender collaborativeRecommender;
        private readonly IHybridRecommender hybridRecommender;

        private readonly IUserBookRepository userBookRepository;

        public IndexModel(IContentBasedRecommender contentBasedRecommender, ICollaborativeRecommender collaborativeRecommender, IHybridRecommender hybridRecommender, IUserBookRepository userBookRepository)
        {
            EnsureArg.IsNotNull(contentBasedRecommender);
            EnsureArg.IsNotNull(collaborativeRecommender);
            EnsureArg.IsNotNull(hybridRecommender);
            EnsureArg.IsNotNull(userBookRepository);

            this.contentBasedRecommender = contentBasedRecommender;
            this.collaborativeRecommender = collaborativeRecommender;
            this.hybridRecommender = hybridRecommender;
            this.userBookRepository = userBookRepository;
        }

        public List<ChartModel> Charts { get; set; } = new List<ChartModel>();

        public string Title { get; set; }

        public int Type { get; set; }

        public void OnGet()
        {
        }

        public async Task OnPostAsync(int type)
        {
            this.Title = type == 1 ? "Average Score" : "Execution Time in second";

            var inputs = await this.userBookRepository.GetByUsernameAsync(this.User.Identity.Name).ConfigureAwait(false);

            var watch = Stopwatch.StartNew();
            var prediction = await this.contentBasedRecommender.GetPredicationsByBooksAsync(inputs.Select(x => x.Book).ToList()).ConfigureAwait(false);
            watch.Stop();

            if (prediction != null)
            {
                this.Charts.Add(new ChartModel() { Label = "CBF", Value = type == 1 ? prediction.Sum(x => x.Score) / prediction.Count() : watch.ElapsedMilliseconds / 1000 });
            }

            watch = Stopwatch.StartNew();
            prediction = await this.collaborativeRecommender.GetPredicationsByBooksAsync(inputs, this.User.Identity.Name).ConfigureAwait(false);
            watch.Stop();

            if (prediction != null)
            {
                this.Charts.Add(new ChartModel() { Label = "CF", Value = type == 1 ? prediction.Sum(x => x.Score) / prediction.Count() : watch.ElapsedMilliseconds / 1000 });
            }

            if (type == 1)
            {
                watch = Stopwatch.StartNew();
                prediction = await this.hybridRecommender.GetPredicationsByBooksAsync(inputs, this.User.Identity.Name).ConfigureAwait(false);
                watch.Stop();

                if (prediction != null)
                {
                    this.Charts.Add(new ChartModel() { Label = "HF", Value = type == 1 ? prediction.Sum(x => x.Score) / prediction.Count() : watch.ElapsedMilliseconds / 1000 });
                }
            }
        }
    }
}