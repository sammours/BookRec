namespace BookRec.Infrastructure.EntityFramework.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BookRec.Infrastructure.EntityFramework.Models;

    public interface IBookRepository : IBaseRepository<Book>
    {
        IQueryable<Book> GetQuery();

        Task<Book> GetByTitleAsync(string title);

        Task<List<Book>> GetByIdsAsync(string[] ids);

        Task<List<ListModel>> GetListByTitleAsync(string value);
    }
}
