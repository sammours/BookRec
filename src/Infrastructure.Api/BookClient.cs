namespace BookRec.Infrastructure.Api
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Threading.Tasks;
    using BookRec.Common;
    using BookRec.Infrastructure.EntityFramework.Models;
    using EnsureThat;
    using Newtonsoft.Json.Linq;

    public class BookClient : IBookClient
    {
        private readonly IHttpClientFactory clientFactory;

        public BookClient(IHttpClientFactory clientFactory)
        {
            EnsureArg.IsNotNull(clientFactory);
            this.clientFactory = clientFactory;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(string query, int page, int maxItems = 40)
        {
            try
            {
                var url = string.Empty;
                if (page == 0)
                {
                    url = $"https://www.googleapis.com/books/v1/volumes?q={query}&maxResults={maxItems}";
                }
                else
                {
                    var startIndex = page * maxItems;
                    url = $"https://www.googleapis.com/books/v1/volumes?q={query}&maxResults={maxItems}&startIndex={startIndex}";
                }

                var request = new HttpRequestMessage(HttpMethod.Get, url);

                var client = this.clientFactory.CreateClient();

                var response = await client.SendAsync(request).ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    throw new HttpRequestException($"Query {query} cannot be found");
                }

                var text = await response.Content.ReadAsStringAsync();
                var json = !string.IsNullOrEmpty(text) ? JToken.Parse(text) : null;
                var result = new List<Book>();
                if (json != null)
                {
                    foreach (var res in json["items"])
                    {
                        var categories = res.GetValuesByPath<string>("volumeInfo.categories[0]");
                        var authors = res.GetValuesByPath<object>("volumeInfo.authors[0]");
                        var book = new Book()
                        {
                            Id = Guid.NewGuid(),
                            Title = res.GetValueByPath<string>("volumeInfo.title"),
                            Subtitle = res.GetValueByPath<string>("volumeInfo.subtitle"),
                            Authors = authors != null ? string.Join(";", authors.ToArray()) : string.Empty,
                            Publisher = res.GetValueByPath<string>("volumeInfo.publisher"),
                            PublishedDate = res.GetValueByPath<DateTime>("volumeInfo.publishedDate"),
                            PageCount = res.GetValueByPath<int>("volumeInfo.pageCount"),
                            Categories = categories != null ? string.Join(";", categories.ToArray()) : string.Empty,
                            MaturityRating = res.GetValueByPath<string>("volumeInfo.maturityRating"),
                            ImageLink = res.GetValueByPath<string>("volumeInfo.imageLinks.thumbnail"),
                            ContainsImageBubbles = res.GetValueByPath<string>("volumeInfo.title"),
                            LanguageCode = res.GetValueByPath<string>("volumeInfo.language"),
                            PrintType = res.GetValueByPath<string>("volumeInfo.printType"),
                            PreviewLink = res.GetValueByPath<string>("volumeInfo.previewLink"),
                            Country = res.GetValueByPath<string>("accessInfo.country"),
                            PublicDomain = res.GetValueByPath<bool>("accessInfo.publicDomain"),
                        };

                        result.Add(book);
                    }
                }

                return result;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"{ex.Message}");
                return null;
            }
        }
    }
}
