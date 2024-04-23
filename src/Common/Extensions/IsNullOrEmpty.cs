namespace BookRec.Common
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;

    public static partial class Extensions
    {
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source) => source == null || !source.Any();

        public static bool IsNullOrEmpty<TSource>(this ICollection<TSource> source) => source == null || !source.Any();

        public static bool IsNullOrEmpty(this Stream source) => source == null || source.Length == 0;
    }
}
