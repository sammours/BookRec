namespace BookRec.Common
{
    using System;

    public static partial class Extensions
    {
        public static bool IsDefault(this object source)
        {
            if (source?.GetType().IsValueType == true)
            {
                switch (source)
                {
                    case int s:
                        return s == default;
                    case long s:
                        return s == default;
                    case double s:
                        return s == default;
                    case decimal s:
                        return s == default;
                    case Guid s:
                        return s == default;
                    // etc: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/value-types
                    default:
                        throw new NotSupportedException($"IsDefault for type {source.GetType().Name}");
                }
            }

            return source == null;
        }

        public static bool IsDefault(this string source)
        {
            return source == default;
        }

        public static bool IsDefault(this int source)
        {
            return source == default;
        }

        public static bool IsDefault(this Guid source)
        {
            return source == default;
        }

        public static bool IsDefault(this DateTime source)
        {
            return source == default;
        }
    }
}
