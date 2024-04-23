namespace BookRec.Common
{
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Extensions
    {
        public static IEnumerable<T> SafeConcat<T>(this IEnumerable<T> source, T @object)
        {
            if (source == null)
            {
                return new List<T> { @object };
            }

            return @object != null ? source.Concat(new List<T> { @object }) : source;
        }

        public static IEnumerable<T> SafeConcat<T>(this IEnumerable<T> source, IEnumerable<T> list)
        {
            if (source == null)
            {
                return list;
            }

            return list != null ? source.Concat(list) : source;
        }

        public static IEnumerable<T> SafeConcat<T>(this IList<T> source, T @object)
        {
            if (source == null)
            {
                return new List<T> { @object };
            }

            return @object != null ? source.Concat(new List<T> { @object }) : source;
        }

        public static IEnumerable<T> SafeConcat<T>(this IList<T> source, IList<T> list)
        {
            if (source == null)
            {
                return list;
            }

            return list != null ? source.Concat(list) : source;
        }

        public static IEnumerable<T> SafeConcat<T>(this T[] source, T[] array)
        {
            if (source == null)
            {
                return array;
            }

            return array != null ? source.Concat(array) : source;
        }

        public static IEnumerable<T> SafeConcat<T>(this T[] source, T obj)
        {
            if (source == null)
            {
                return new List<T>() { obj };
            }

            return obj != null ? source.Concat(new List<T>() { obj }) : source;
        }
    }
}
