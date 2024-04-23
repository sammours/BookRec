namespace BookRec.Infrastructure.EntityFramework.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using BookRec.Common;
    using BookRec.Infrastructure.EntityFramework.Context;
    using BookRec.Infrastructure.EntityFramework.Extensions;
    using BookRec.Infrastructure.EntityFramework.Models;
    using EFCore.BulkExtensions;
    using EnsureThat;
    using Microsoft.EntityFrameworkCore;

    public class BaseRepository<T> : IBaseRepository<T>
        where T : AggregateRoot
    {
        private readonly BookRecContext dbContext;

        public BaseRepository(BookRecContext dbContext)
        {
            EnsureArg.IsNotNull(dbContext, nameof(dbContext));

            this.dbContext = dbContext;
        }

        public BookRecContext DbContext { get => this.dbContext; }

        public async Task<bool> DeleteAsync(Guid id)
        {
            EnsureArg.IsNotNull<Guid>(id);
            var entity = await this.dbContext.Set<T>()
                .FirstOrDefaultAsync(x => x.Id == id).ConfigureAwait(false);
            if (entity != null)
            {
                this.dbContext.Remove(entity);
                await this.dbContext.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<T>> GetAllAsync()
            => await this.dbContext.Set<T>().ToListAsync().ConfigureAwait(false);

        public async Task<T> GetByIdAsync(Guid id)
        {
            EnsureArg.IsNotNull<Guid>(id);
            return await this.dbContext.Set<T>().FirstOrDefaultAsync(x => x.Id == id)
                .ConfigureAwait(false);
        }

        public async Task<T> InsertAsync(T model)
        {
            EnsureArg.IsNotNull<T>(model);
            return await this.UpsertAsync(model).ConfigureAwait(false);
        }

        public async Task<List<T>> InsertBulkAsync(List<T> models)
        {
            EnsureArg.IsNotNull(models);
            await this.dbContext.BulkInsertAsync(models).ConfigureAwait(false);
            return models;
        }

        public async Task<bool> ExistsAsync(Guid id)
        {
            EnsureArg.IsNotNull<Guid>(id);
            return await this.GetByIdAsync(id).ConfigureAwait(false) != null;
        }

        public async Task<T> UpdateAsync(T model)
        {
            EnsureArg.IsNotNull<T>(model);
            return await this.UpsertAsync(model).ConfigureAwait(false);
        }

        public async Task<T> UpsertAsync(T entity)
        {
            EnsureArg.IsNotNull(entity);
            var isNew = entity.Id.IsDefault() || !await this.ExistsAsync(entity.Id).ConfigureAwait(false);

            if (isNew)
            {
                this.dbContext.Set<T>().Add(entity);
            }

            await this.dbContext.SaveChangesAsync<T>().ConfigureAwait(false);

            return entity;
        }
    }
}
