namespace BookRec.Tools.Migration
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using BookRec.Infrastructure.EntityFramework.Models;
    using Infrastructure.Api;
    using Infrastructure.EntityFramework.Repositories;
    using Microsoft.Extensions.DependencyInjection;

    public static class BookMigration
    {
        public static async Task StartAsync(ServiceProvider serviceProvider)
        {
            var client = serviceProvider.GetService<IBookClient>();
            var repositopry = serviceProvider.GetService<IBookRepository>();

            foreach (var query in BookCategories.GetSearchQueries2().OrderByDescending(x => x))
            {
                Console.WriteLine($"Get books for {query}");
                var tasks = new List<Task>();
                var books1 = client.GetBooksAsync(HttpUtility.HtmlEncode(query), 0);
                var books2 = client.GetBooksAsync(HttpUtility.HtmlEncode(query), 1);
                var books3 = client.GetBooksAsync(HttpUtility.HtmlEncode(query), 2);
                var books4 = client.GetBooksAsync(HttpUtility.HtmlEncode(query), 3);
                var books5 = client.GetBooksAsync(HttpUtility.HtmlEncode(query), 4);
                var books6 = client.GetBooksAsync(HttpUtility.HtmlEncode(query), 5);
                var books7 = client.GetBooksAsync(HttpUtility.HtmlEncode(query), 6);
                var books8 = client.GetBooksAsync(HttpUtility.HtmlEncode(query), 7);
                var books9 = client.GetBooksAsync(HttpUtility.HtmlEncode(query), 8);
                var books10 = client.GetBooksAsync(HttpUtility.HtmlEncode(query), 9);

                var books = new List<Book>();
                await Task.WhenAll(books1, books2, books3, books4, books5, books6, books7, books8, books9, books10).ConfigureAwait(false);
                var list = await books1.ConfigureAwait(false);
                if (list != null)
                {
                    books.AddRange(list);
                }

                list = await books2.ConfigureAwait(false);
                if (list != null)
                {
                    books.AddRange(list);
                }

                list = await books3.ConfigureAwait(false);
                if (list != null)
                {
                    books.AddRange(list);
                }

                list = await books4.ConfigureAwait(false);
                if (list != null)
                {
                    books.AddRange(list);
                }

                list = await books5.ConfigureAwait(false);
                if (list != null)
                {
                    books.AddRange(list);
                }

                list = await books6.ConfigureAwait(false);
                if (list != null)
                {
                    books.AddRange(list);
                }

                list = await books7.ConfigureAwait(false);
                if (list != null)
                {
                    books.AddRange(list);
                }

                list = await books8.ConfigureAwait(false);
                if (list != null)
                {
                    books.AddRange(list);
                }

                list = await books9.ConfigureAwait(false);
                if (list != null)
                {
                    books.AddRange(list);
                }

                list = await books10.ConfigureAwait(false);
                if (list != null)
                {
                    books.AddRange(list);
                }

                var booksToBeSaves = new List<Book>();
                if (books != null && books.Any())
                {
                    Console.WriteLine($"Book Found: {books.Count()}");
                    foreach (var book in books)
                    {
                        try
                        {
                            var model = await repositopry.GetByTitleAsync(book.Title).ConfigureAwait(false);
                            if (model == null)
                            {
                                booksToBeSaves.Add(book);
                                Console.WriteLine($"{book.Title} will be saved");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                }

                if (booksToBeSaves != null && booksToBeSaves.Any())
                {
                    try
                    {
                        booksToBeSaves = booksToBeSaves.Where(x => x != null).ToList();
                        await repositopry.InsertBulkAsync(booksToBeSaves).ConfigureAwait(false);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
