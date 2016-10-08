namespace SimplerScore.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Reflection;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Attributes;
    using Microsoft.Owin;

    internal static class ErrorHandlingExtensions
    {
        /// <summary>
        /// Maps the error to HTTP status.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="map">The map.</param>
        /// <returns></returns>
        internal static int MapErrorToHttpStatus (this Exception ex, IDictionary<Type, HttpStatusCode> map)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            var errorType = ex.GetType();

            if (map.ContainsKey(errorType))
            {
                statusCode = map[errorType];
            }

            return (int)statusCode;
        }
    }

    internal class ErrorHandlingMiddleware : OwinMiddleware
    {
        #region Constructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="ErrorHandlingMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public ErrorHandlingMiddleware (OwinMiddleware next)
            : base(next)
        {
        }
        #endregion

        #region Public methods        
        /// <summary>
        /// Invokes the pipeline operation and advances to next one.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public override async Task Invoke (IOwinContext context)
        {
            try
            {
                // Pass everything up through the pipeline first:
                await Next.Invoke(context);
            }
            catch (HttpResponseException ex)
            {
                context.Response.StatusCode = (int)ex.Response.StatusCode;
                var content = await ex.Response.Content.ReadAsByteArrayAsync();
                context.Response.Write(content);
            }
            catch (Exception ex)
            {
                var attr = ex.GetType().GetCustomAttribute<HttpStatusAttribute>();

                if (attr != null)
                    context.Response.StatusCode = (int) attr.Status;
                else
                {
                    var exceptionToErrorCodeMap = OnBuildErrorCodeMap();

                    var statusCode = ex.MapErrorToHttpStatus(exceptionToErrorCodeMap);
                    context.Response.StatusCode = statusCode;
                }

                context.Response.Write(ex.Message);
            }
        }
        #endregion

        #region Protected methods
        protected virtual IDictionary<Type, HttpStatusCode> OnBuildErrorCodeMap ()
        {
            return new Dictionary<Type, HttpStatusCode>();
        }
        #endregion
    }
}
