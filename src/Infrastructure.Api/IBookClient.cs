namespace BookRec.Infrastructure.Api
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BookRec.Infrastructure.EntityFramework.Models;

    public interface IBookClient
    {
        Task<IEnumerable<Book>> GetBooksAsync(string query, int page, int maxItems = 40);
    }
}