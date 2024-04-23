namespace BookRec.Tests.UnitTests
{
    using System;
    using BookRec.Recommender;
    using Infrastructure.Api;
    using Infrastructure.EntityFramework;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class Startup
    {
        public static IServiceProvider Start()
        {
            var builder = new ConfigurationBuilder();

            var configuration = builder.Build();
            var services = new ServiceCollection();
            var serviceProvider = services
                .AddSingleton(sc => configuration)
                .AddHttpClient()
                .AddClients()
                .AddRepositories()
                .AddRecommenders()
                .BuildServiceProvider();

            return serviceProvider;
        }
    }
}