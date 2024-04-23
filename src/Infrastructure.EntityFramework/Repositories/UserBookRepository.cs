namespace BookRec.Infrastructure.EntityFramework.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BookRec.Common;
    using BookRec.Infrastructure.EntityFramework.Context;
    using BookRec.Infrastructure.EntityFramework.Models;
    using EFCore.BulkExtensions;
    using EnsureThat;
    using Microsoft.EntityFrameworkCore;

    public class UserBookRepository : BaseRepository<UserBook>, IUserBookRepository
    {
        public UserBookRepository(BookRecContext dbContext)
            : base(dbContext)
        {
        }

        public IQueryable<UserBook> GetQuery() => this.DbContext.UserBooks;

        public async Task<List<UserBook>> GetByUsernameAsync(string username)
        {
            EnsureArg.IsNotNullOrEmpty(username);
            return await this.DbContext.UserBooks.Include(x => x.Book).Where(x => x.Username == username).ToListAsync().ConfigureAwait(false);
        }

        public async Task<bool> DeleteByBookIdAsync(string bookId, string username)
        {
            EnsureArg.IsNotNullOrEmpty(bookId);
            EnsureArg.IsNotNullOrEmpty(username);

            var book = await this.DbContext.UserBooks.FirstOrDefaultAsync(x => x.BookId == bookId.ToGuid().Value && x.Username == username).ConfigureAwait(false);
            if (book != null)
            {
                return await this.DeleteAsync(book.Id).ConfigureAwait(false);
            }

            return false;
        }

        public async Task<UserBook> GetByBookIdAsync(string bookId, string username)
        {
            EnsureArg.IsNotNullOrEmpty(bookId);
            EnsureArg.IsNotNullOrEmpty(username);

            var book = await this.DbContext.UserBooks.FirstOrDefaultAsync(x => x.BookId == bookId.ToGuid().Value && x.Username == username).ConfigureAwait(false);
            if (book != null)
            {
                return book;
            }

            return null;
        }

        public async Task<UserBook> UpdateStarAsync(string id, string username, int stars)
        {
            EnsureArg.IsNotNullOrEmpty(id);
            stars = stars > 5 ? 5 : stars < 0 ? 0 : stars;
            var dbObj = await this.GetByIdAsync(id.ToGuid().Value).ConfigureAwait(false);
            if (dbObj == null)
            {
                throw new ArgumentNullException($"{nameof(UserBook)} {id} not found");
            }

            if (dbObj.Username != username)
            {
                throw new UnauthorizedAccessException($"User {username} is not authorized to edit {nameof(UserBook)} {id}");
            }

            dbObj.Rating = stars;
            return await this.UpdateAsync(dbObj).ConfigureAwait(false);
        }
    }
}
