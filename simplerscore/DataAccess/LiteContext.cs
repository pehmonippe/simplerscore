namespace SimplerScore.DataAccess
{
    using System;
    using LiteDB;

    internal class LiteContext : IDisposable
    {
        public LiteDatabase Db
        {
            get;
        }

        public LiteContext (LiteDatabase db)
        {
            Db = db;
        }

        ~LiteContext ()
        {
            Dispose(false);
        }

        public LiteCollection<T> Collection<T> (string name = null)
            where T : class, new()
        {
            var collectionName = name ?? $"{typeof (T).Name}s";

            var col = Db.GetCollection<T>(collectionName);
            return col;
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
            Db.Dispose();
        }
        #endregion
    }


    //internal class LiteContext<T> : IDisposable
    //    where T : class, new()
    //{
    //    private readonly LiteDatabase db;

    //    public LiteDatabase Db
    //    {
    //        get
    //        {
    //            ThrowIfDisposed();
    //            return db;
    //        }
    //    }

    //    private LiteCollection<T> collection;

    //    public LiteCollection<T> Collection
    //    {
    //        get
    //        {
    //            ThrowIfDisposed();

    //            return collection ?? (collection = GetCollection());
    //        }
    //    }

    //    public LiteContext (LiteDatabase db)
    //    {
    //        this.db = db;
    //    }

    //    private LiteCollection<T> GetCollection ()
    //    {
    //        var col = Db.GetCollection<T>($"{typeof(T).Name}s");
    //        return col;
    //    }

    //    #region IDisposable implementation
    //    #region Pattern body
    //    /// <summary>
    //    /// Internal flag of current dispose state.
    //    /// </summary>
    //    private bool disposed = false;

    //    /// <summary>
    //    /// IDisposable interface method.
    //    /// </summary>
    //    public void Dispose ()
    //    {
    //        Dispose(true);
    //        GC.SuppressFinalize(this);
    //    }

    //    /// <summary>
    //    /// Helper method for testing dispose status
    //    /// </summary>
    //    protected void ThrowIfDisposed ()
    //    {
    //        if (!disposed)
    //            return;

    //        throw new ObjectDisposedException(GetType().Name);
    //    }

    //    /// <summary>
    //    /// Main body of the disposable pattern.
    //    /// </summary>
    //    /// <param name="disposing">true, if executing explicit dispose, false if implicit like runned from destructor.</param>
    //    protected void Dispose (bool disposing)
    //    {
    //        if (disposed)
    //        {
    //            return;
    //        }

    //        if (disposing)
    //        {
    //            OnDispose();
    //        }

    //        disposed = true;
    //    }
    //    #endregion

    //    /// <summary>
    //    /// Custom dispose implementation for the dispose pattern.
    //    /// </summary>
    //    protected virtual void OnDispose ()
    //    {
    //        db.Dispose();
    //    }
    //    #endregion
    //}
}