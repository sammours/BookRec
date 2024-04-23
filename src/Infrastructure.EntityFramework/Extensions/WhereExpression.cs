namespace BookRec.Infrastructure.EntityFramework.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public static partial class Extensions
    {
        public static IQueryable<TSource> WhereExpression<TSource>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> expression)
        {
            if (expression != null)
            {
                return source.Where(expression);
            }

            return source;
        }

        public static IQueryable<TSource> WhereExpressions<TSource>(
            this IQueryable<TSource> source,
            IEnumerable<Expression<Func<TSource, bool>>> expressions)
        {
            var expressionsArray = expressions as Expression<Func<TSource, bool>>[] ?? expressions?.ToArray();
            if (expressionsArray?.Length > 0)
            {
                foreach (var expression in expressionsArray)
                {
                    source = source.Where(expression);
                }
            }

            return source;
        }
    }
}
