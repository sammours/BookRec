namespace BookRec.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public static partial class Extensions
    {
        public static Func<T, bool> ToPredicate<T>(this Expression<Func<T, bool>> expression)
            => expression?.Compile();
    }
}
