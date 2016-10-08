namespace SimplerScore.Middleware
{
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Utilities;

    public class LoggingMiddleware : OwinMiddleware
    {
        #region Constructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public LoggingMiddleware (OwinMiddleware next)
            : base(next)
        {
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Invokes the specified environment.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override async Task Invoke (IOwinContext context)
        {
            var method = context.Request.Method;
            var path = context.Request.Path;
            var scopeName = $"{path} [{method}]";

            using (var scope = new LoggingScope(scopeName))
            {
                await Next.Invoke(context);
                scope.Log("Status Code: {0}", context.Response.StatusCode);
            }
        }
        #endregion
    }
}
