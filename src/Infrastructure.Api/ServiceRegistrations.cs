namespace BookRec.Infrastructure.Api
{
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static partial class ServiceRegistrations
    {
        /// <summary>
        /// Adds BookRec Clients
        /// </summary>
        /// <param name="services">The services</param>
        /// <returns>Service Collection</returns>
        public static IServiceCollection AddClients(this IServiceCollection services)
        {
            services.TryAddScoped<IBookClient, BookClient>();
            return services;
        }
    }
}
