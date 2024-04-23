namespace BookRec.Infrastructure.EntityFramework.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using BookRec.Infrastructure.EntityFramework.Context;
    using BookRec.Infrastructure.EntityFramework.Models;

    public interface IBaseRepository<T>
        where T : AggregateRoot
    {
        BookRecContext DbContext { get; }

        Task<bool> DeleteAsync(Guid id);

        Task<IEnumerable<T>> GetAllAsync();

        Task<T> GetByIdAsync(Guid id);

        Task<T> InsertAsync(T model);

        Task<List<T>> InsertBulkAsync(List<T> models);

        Task<bool> ExistsAsync(Guid id);

        Task<T> UpdateAsync(T model);

        Task<T> UpsertAsync(T entity);
    }
}
