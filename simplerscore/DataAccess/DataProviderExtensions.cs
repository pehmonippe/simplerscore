namespace SimplerScore.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    /// <summary>
    /// Specialized extension methods to perform basic non-transactional
    /// CRUD operations on single data type.
    /// </summary>
    internal static class DataProviderExtensions
    {
        public static T Find<T> (this IDataProvider tdp, int id)
            where T : class, new()
        {
            var item = tdp.Collection<T>().FindById(id);
            return item;
        }

        public static List<T> Find<T> (this IDataProvider tdp, Expression<Func<T, bool>> predicate, int skip = 0, int limit = int.MaxValue)
            where T : class, new()
        {
            var result = tdp.Collection<T>()
                .Find(predicate, skip, limit)
                .ToList();

            return result;
        }

        public static List<T> FindAll<T> (this IDataProvider tdp)
            where T : class, new()
        {
            var all = tdp.Collection<T>()
                .FindAll()
                .ToList();

            return all;
        }

        public static int Add<T> (this IDataProvider tdp, T item)
            where T : class, new()
        {
            var value = tdp.Collection<T>().Insert(item);
            return value;
        }

        public static void Update<T> (this IDataProvider tdp, T item)
            where T : class, new()
        {
            tdp.Collection<T>().Update(item);
        }

        public static bool Delete<T> (this IDataProvider tdp, int itemId)
            where T : class, new()
        {
            var deleted = tdp.Collection<T>().Delete(itemId);
            return deleted;
        }

        public static int Delete<T> (this IDataProvider tdp, Expression<Func<T, bool>> predicate)
            where T : class, new()
        {
            var deletedCount = tdp.Collection<T>().Delete(predicate);
            return deletedCount;
        }
    }
}