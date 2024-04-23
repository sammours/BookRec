namespace BookRec.Infrastructure.EntityFramework.Extensions
{
    using System.Linq;
    using System.Threading.Tasks;
    using BookRec.Infrastructure.EntityFramework.Models;
    using EnsureThat;
    using Microsoft.EntityFrameworkCore;

    /// <summary>
    /// DbContext extensions.
    /// </summary>
    public static partial class Extensions
    {
        /// <summary>
        /// <para>
        /// Saves the changes only for the TEntity type (Aggregate). All other aggregate instances
        /// are ignored and not saved.
        /// </para>
        /// <para>
        /// Preventing SaveChanges for other aggregates can also be prevented by using
        /// seperate DbContext instances.
        /// </para>
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity to save.</typeparam>
        /// <param name="source">The source.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task<int> SaveChangesAsync<TEntity>(this DbContext source)
            where TEntity : AggregateRoot
        {
            EnsureArg.IsNotNull(source, nameof(source));

            // find all other aggregates
            var other = source.ChangeTracker.Entries()
                .Where(x => !typeof(TEntity).IsAssignableFrom(x.Entity.GetType())
                    && x.State != EntityState.Unchanged)
                .GroupBy(x => x.State)
                .ToList();

            // set all other aggregates to unchanged
            foreach (var entry in source.ChangeTracker.Entries()
                .Where(x => !typeof(TEntity).IsAssignableFrom(x.Entity.GetType())))
            {
                // WARN: this is not 100% fool proof: as other modified aggregate child entities are not marked as unchanged
                entry.State = EntityState.Unchanged;
            }

            var result = await source.SaveChangesAsync().ConfigureAwait(false);

            // set all other aggregates to original state
            foreach (var state in other)
            {
                foreach (var entry in state)
                {
                    entry.State = state.Key;
                }
            }

            return result;
        }
    }
}
