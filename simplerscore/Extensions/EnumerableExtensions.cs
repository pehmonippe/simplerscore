namespace SimplerScore.Extensions
{
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;

    public static class EnumerableExtensions
    {
        public static bool IsNullOrEmpty<T> ([CanBeNull] this IEnumerable<T> enumerable)
        {
            if (null == enumerable)
                return true;

            var isEmpty = enumerable
                .AsList()
                .Any();

            return isEmpty;
        }

        public static List<T> AsList<T> ([NotNull] this IEnumerable<T> enumerable)
        {
            return enumerable as List<T> ?? enumerable.ToList();
        }
    }
}
