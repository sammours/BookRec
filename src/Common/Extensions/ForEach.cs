﻿namespace BookRec.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Extensions
    {
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source.IsNullOrEmpty())
            {
                return source;
            }

            var itemsArray = source as T[] ?? source.ToArray();

            foreach (var value in itemsArray)
            {
                if (action != null && !EqualityComparer<T>.Default.Equals(value, default(T)))
                {
                    action(value);
                }
            }

            return itemsArray;
        }

        public static ICollection<T> ForEach<T>(this ICollection<T> source, Action<T> action)
        {
            return source.AsEnumerable().ForEach(action).ToList();
        }

        public static IReadOnlyCollection<T> ForEach<T>(this IReadOnlyCollection<T> source, Action<T> action)
        {
            return source.AsEnumerable().ForEach(action).ToList();
        }

        public static void ForEach<T>(this IEnumerable<T> source, Func<T, IEnumerable<T>> childSelector, Action<T> action = null)
        {
            if (source.IsNullOrEmpty())
            {
                return;
            }

            if (action == null)
            {
                return;
            }

            foreach (var item in source)
            {
                action(item);
                childSelector?.Invoke(item).ForEach(childSelector, action);
            }
        }
    }
}
