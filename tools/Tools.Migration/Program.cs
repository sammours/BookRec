namespace BookRec.Tools.Migration
{
    using System;
    using System.Threading.Tasks;
    using Infrastructure.Api;
    using Infrastructure.EntityFramework;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = new ConfigurationBuilder();

            var configuration = builder.Build();
            var services = new ServiceCollection();
            var serviceProvider = services
                .AddSingleton(sc => configuration)
                .AddHttpClient()
                .AddDatabase()
                .AddClients()
                .AddRepositories()
                .BuildServiceProvider();

            Console.WriteLine("Start!!!");
            await BookMigration.StartAsync(serviceProvider).ConfigureAwait(false);
        }
    }
}
