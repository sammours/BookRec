namespace BookRec.Infrastructure.EntityFramework.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BookRec.Infrastructure.EntityFramework.Context;
    using BookRec.Infrastructure.EntityFramework.Models;
    using EnsureThat;
    using Microsoft.EntityFrameworkCore;

    public class BookRepository : BaseRepository<Book>, IBookRepository
    {
        public BookRepository(BookRecContext dbContext)
            : base(dbContext)
        {
        }

        public IQueryable<Book> GetQuery() => this.DbContext.Books;

        public async Task<Book> GetByTitleAsync(string title)
        {
            EnsureArg.IsNotNullOrEmpty(title);
            return await this.DbContext.Books.FirstOrDefaultAsync(x => x.Title == title).ConfigureAwait(false);
        }

        public async Task<List<Book>> GetByIdsAsync(string[] ids)
            => await this.DbContext.Books.Where(x => ids.Contains(x.Id.ToString())).ToListAsync().ConfigureAwait(false);

        public async Task<List<ListModel>> GetListByTitleAsync(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return new List<ListModel>();
            }

            return await this.DbContext.Books.Where(x => x.Title.Contains(value)).Select(x => new ListModel() { Value = x.Id.ToString(), Text = x.Title })
                .OrderBy(x => x.Value).Take(10).ToListAsync();
        }
    }
}
