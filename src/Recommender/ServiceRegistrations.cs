namespace BookRec.Recommender
{
    using BookRec.Infrastructure.EntityFramework.Repositories;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static partial class ServiceRegistrations
    {
        /// <summary>
        /// Adds BookRec Recommenders
        /// </summary>
        /// <param name="services">The services</param>
        /// <returns>Service Collection</returns>
        public static IServiceCollection AddRecommenders(this IServiceCollection services)
        {
            services.TryAddScoped<IContentBasedRecommender>(sp =>
            {
                var repository = sp.GetService<IBookRepository>();
                return new ContentBasedRecommender(repository);
            });

            services.TryAddScoped<ICollaborativeRecommender>(sp =>
            {
                var repository = sp.GetService<IUserBookRepository>();
                return new CollaborativeRecommender(repository);
            });

            services.TryAddScoped<IHybridRecommender>(sp =>
            {
                var cbf = sp.GetService<IContentBasedRecommender>();
                var cf = sp.GetService<ICollaborativeRecommender>();
                return new HybridRecommender(cbf, cf);
            });

            return services;
        }
    }
}
