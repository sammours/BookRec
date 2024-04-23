namespace BookRec.Common
{
    using System;

    public static partial class Extensions
    {
        public static Guid? ToGuid(this string source)
        {
            if (source.IsNullOrEmpty())
            {
                return null;
            }

            if (Guid.TryParse(source, out Guid parsedValue))
            {
                return parsedValue;
            }

            return null;
        }
    }
}
