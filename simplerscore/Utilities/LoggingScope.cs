namespace SimplerScore.Utilities
{
    using System;
    using System.Diagnostics;

    public class LoggingScope : IDisposable
    {
        #region Fields
        private readonly Stopwatch stopwatch;
        private readonly string scope;

        /// <summary>
        /// Logger instance.
        /// </summary>
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #endregion

        #region Constructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingScope"/> class.
        /// </summary>
        /// <param name="scope">The scope.</param>
        public LoggingScope (string scope)
        {
            this.scope = scope;
            stopwatch = new Stopwatch();
            stopwatch.Start();
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="LoggingScope"/> class.
        /// </summary>
        ~LoggingScope ()
        {
            Dispose(false);
        }
        #endregion

        #region Public methods        
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public LoggingScope Log (string message, params object[] args)
        {
            if (Logger.IsInfoEnabled)
                Logger.InfoFormat(scope + ": " + message, args);
            
            return this;
        }
        #endregion

        #region IDisposable Support
        private bool disposedValue; // To detect redundant calls

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose (bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    stopwatch.Stop();
                    if (Logger.IsInfoEnabled)
                        Logger.InfoFormat($"Processing scope {scope} took {stopwatch.Elapsed}.{Environment.NewLine}");
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose ()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
