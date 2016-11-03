namespace SimplerScore.Utilities
{
    using System;

    public class Disposable : IDisposable
    {
        #region IDisposable implementation
        #region Pattern body
        /// <summary>
        /// Internal flag of current dispose state.
        /// </summary>
        protected bool Disposed;

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
            if (!Disposed)
                return;

            throw new ObjectDisposedException(GetType().Name);
        }

        /// <summary>
        /// Main body of the disposable pattern.
        /// </summary>
        /// <param name="disposing">true, if executing explicit dispose, false if implicit like runned from destructor.</param>
        protected void Dispose (bool disposing)
        {
            if (Disposed)
            {
                return;
            }

            if (disposing)
            {
                OnDispose();
            }

            Disposed = true;
        }
        #endregion

        /// <summary>
        /// Custom dispose implementation for the dispose pattern.
        /// </summary>
        protected virtual void OnDispose ()
        {
            //TODO: add class specific disposing here.  

        }
        #endregion
    }
}
