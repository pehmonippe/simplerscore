namespace SimplerScore.Extensions
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Middleware;
    using NetTools;
    using Owin;

    public static class MiddlewareExtensions
    {
        /// <summary>
        /// Extension method to assing logging middleware into pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void UseLoggingMiddleware ([NotNull] this IAppBuilder app)
        {
            app.Use<LoggingMiddleware>();
        }

        /// <summary>
        /// Extension method to assign error handling middleware into pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void UseErrorHandlingMiddleware ([NotNull] this IAppBuilder app)
        {
            app.Use<ErrorHandlingMiddleware>();
        }

        /// <summary>
        /// Extension method to assign authorization middleware into pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        public static void UseAuthorizationMiddleware ([NotNull] this IAppBuilder app)
        {
            app.Use<AuthorizationMiddleware>();
        }

        /// <summary>
        /// Extension method to assign connection filtering middleware into pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="whiteList">The white list.</param>
        /// <param name="blackList">The black list.</param>
        public static void UseConnectionFilterMiddleware ([NotNull] this IAppBuilder app, HashSet<IPAddressRange> whiteList = null, HashSet<IPAddressRange> blackList = null)
        {
            app.Use<ConnectionFilterMiddleware>(whiteList, blackList);
        }
    }
}
