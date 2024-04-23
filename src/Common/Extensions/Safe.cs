﻿namespace BookRec.Common
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public static partial class Extensions
    {
        /// <summary>
        /// Converts a null list to an empty list. Also clears out possible 'null' items
        /// Avoids null ref exceptions
        /// </summary>
        /// <typeparam name="TSource">the source</typeparam>
        /// <param name="source">the source collection</param>
        /// <returns>collection of sources</returns>
        public static IEnumerable<TSource> Safe<TSource>(this IEnumerable<TSource> source)
        {
            return (source ?? Enumerable.Empty<TSource>()).Where(i => i != null);
        }

        public static string Safe(this string source)
        {
            return source ?? string.Empty;
        }

        /// <summary>
        /// Converts an null list to an empty list. Also clears out possible 'null' items
        /// Avoids null ref exceptions
        /// </summary>
        /// <typeparam name="TSource">the source</typeparam>
        /// <param name="source">the source collection</param>
        /// <returns>collection of sources</returns>
        public static ICollection<TSource> Safe<TSource>(this ICollection<TSource> source)
        {
            return (source ?? new Collection<TSource>()).Where(i => i != null).ToList();
        }

        /// <summary>
        /// Converts an null dictionary to an empty dictionary. avoids null ref exceptions
        /// </summary>
        /// <typeparam name="TKey">the source key type</typeparam>
        /// <typeparam name="TValue">the source value type</typeparam>
        /// <param name="source">the source collection</param>
        /// <returns>collection of sources</returns>
        public static IDictionary<TKey, TValue> Safe<TKey, TValue>(this IDictionary<TKey, TValue> source)
        {
            return source ?? new Dictionary<TKey, TValue>();
        }
    }
}
