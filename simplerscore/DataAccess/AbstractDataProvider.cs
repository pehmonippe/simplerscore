namespace SimplerScore.DataAccess
{
    using System.Collections.Generic;
    using System.Linq;
    using LiteDB;

    public interface IDataProvider<T>
        where T : class, new()
    {
        T Find (int id);

        IEnumerable<T> GetAll ();

        int Add (T item);

        void Update (T item);

        void Delete (int itemId);
    }


    internal abstract class AbstractDataProvider<T> : IDataProvider<T>
        where T : class, new()
    {
        public T Find (int id)
        {
            using (var context = CreateContext())
            {
                var item = context.Collection.FindById(id);
                return item;
            }
        }

        public IEnumerable<T> GetAll ()
        {
            using (var context = CreateContext())
            {
                var all = context.Collection.FindAll().ToList();
                return all;
            }
        }

        public int Add (T item)
        {
            using (var context = CreateContext())
            {
                var value = context.Collection.Insert(item);

                return value;
            }
        }

        public void Update (T item)
        {
            using (var context = CreateContext())
            {
                context.Collection.Update(item);
            }
        }

        public void Delete (int itemId)
        {
            using (var context = CreateContext())
            {
                context.Collection.Delete(itemId);
            }
        }

        protected LiteContext<T> CreateContext ()
        {
            var db = CreateDatabase();
            var liteContext = new LiteContext<T>(db);

            return liteContext;
        }

        private static LiteDatabase CreateDatabase ()
        {
            var db = new LiteDatabase(@"simplerscore.db");
            return db;
        }
    }
}