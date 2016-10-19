namespace SimplerScore.DataAccess
{
    using LiteDB;
    using System;
    using JetBrains.Annotations;

    /// <summary>
    /// Data access provider with transaction support.
    /// </summary>
    internal class DataProvider : IDataProvider
    {
        private readonly string dbInstanceName;
        private LiteContext context;

        public DataProvider (string dbInstanceName)
        {
            this.dbInstanceName = dbInstanceName;
        }

        public void Initialize ()
        {
            ThrowIfDisposed();

            CreateContext();
        }

        public LiteCollection<T> Collection<T>()
            where T : class, new()
        {
            ThrowIfDisposed();

            var col = context.Collection<T>();
            return col;
        }

        public IDisposable BeginScope ()
        {
            ThrowIfDisposed();

            return context.Db.BeginTrans();
        }

        public void Commit ([NotNull] IDisposable disposable)
        {
            ThrowIfDisposed();

            var transaction = disposable as LiteTransaction;
            transaction?.Commit();
        }

        protected void CreateContext ()
        {
            var db = new LiteDatabase(dbInstanceName);
            context = new LiteContext(db);
        }

        #region IDisposable implementation
        #region Pattern body
        /// <summary>
        /// Internal flag of current dispose state.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// IDisposable interface method.
        /// </summary>
        public void Dispose ()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Helper method for testing dispose status
        /// </summary>
        protected void ThrowIfDisposed ()
        {
            if (!disposed)
                return;

            throw new ObjectDisposedException(GetType().Name);
        }

        /// <summary>
        /// Main body of the disposable pattern.
        /// </summary>
        /// <param name="disposing">true, if executing explicit dispose, false if implicit like runned from destructor.</param>
        protected void Dispose (bool disposing)
        {
            if (disposed)
            {
                return;
            }

            if (disposing)
            {
                OnDispose();
            }

            disposed = true;
        }
        #endregion

        /// <summary>
        /// Custom dispose implementation for the dispose pattern.
        /// </summary>
        protected virtual void OnDispose ()
        {
            context?.Dispose();
            context = null;

        }
        #endregion
    }
}