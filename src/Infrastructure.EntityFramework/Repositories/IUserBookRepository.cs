namespace BookRec.Infrastructure.EntityFramework.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BookRec.Infrastructure.EntityFramework.Models;

    public interface IUserBookRepository : IBaseRepository<UserBook>
    {
        Task<List<UserBook>> GetByUsernameAsync(string username);

        Task<UserBook> GetByBookIdAsync(string bookId, string username);

        Task<UserBook> UpdateStarAsync(string id, string username, int stars);

        Task<bool> DeleteByBookIdAsync(string bookId, string username);

        IQueryable<UserBook> GetQuery();
    }
}
