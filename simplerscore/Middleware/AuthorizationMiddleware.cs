namespace SimplerScore.Middleware
{
    using System.Threading.Tasks;
    using Microsoft.Owin;

    public class AuthorizationMiddleware : OwinMiddleware
    {
        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizationMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        public AuthorizationMiddleware (OwinMiddleware next)
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
            OnAuthorization(context);

            // advance to next middleware in Katana pipeline.
            await Next.Invoke(context);
        }
        #endregion

        #region Protected methods

        /// <summary>
        /// Called when authorization takes place.
        /// </summary>
        /// <remarks>
        /// Method should focus on authorization routine. 
        /// It is not the intent of this method to invoke next method in pipeline.
        /// </remarks>
        /// <param name="context">The context.</param>
        protected virtual void OnAuthorization (IOwinContext context)
        {
        }
        #endregion
    }
}
