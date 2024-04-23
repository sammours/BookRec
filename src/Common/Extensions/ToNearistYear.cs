namespace BookRec.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static partial class Extensions
    {
        public static int ToNearistCentury(this DateTime source)
            => ((int)(source.Year / 100) + (source.Year % 100 == 0 ? 0 : 1)) * 100;
    }
}
