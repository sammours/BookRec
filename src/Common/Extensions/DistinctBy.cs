namespace BookRec.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Extensions
    {
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            return items.GroupBy(property).Select(x => x.First());
        }
    }
}
