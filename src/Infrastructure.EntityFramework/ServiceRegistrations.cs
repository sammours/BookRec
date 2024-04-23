namespace BookRec.Infrastructure.EntityFramework
{
    using System;
    using BookRec.Infrastructure.EntityFramework.Context;
    using BookRec.Infrastructure.EntityFramework.Repositories;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;

    public static partial class ServiceRegistrations
    {
        /// <summary>
        /// Adds BookRec Repositories
        /// </summary>
        /// <param name="services">The services</param>
        /// <returns>Service Collection</returns>
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.TryAddScoped<IBookRepository>(sp =>
            {
                var context = sp.GetService<BookRecContext>();
                return new BookRepository(context);
            });

            services.TryAddScoped<IUserBookRepository>(sp =>
            {
                var context = sp.GetService<BookRecContext>();
                return new UserBookRepository(context);
            });

            return services;
        }

        /// <summary>
        /// Adds BookRec database
        /// </summary>
        /// <param name="services">The services</param>
        /// <returns>Service Collection</returns>
        public static IServiceCollection AddDatabase(this IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();

            var configuration = serviceProvider.GetService<IConfiguration>();
            string connectionString = Environment.GetEnvironmentVariable("BookRec:DatabaseConnectionString");

            services.AddDbContext<BookRecContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }
    }
}
