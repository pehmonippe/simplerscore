namespace SimplerScore.Middleware
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Microsoft.Owin;
    using NetTools;

    public class ConnectionFilterMiddleware : OwinMiddleware
    {
        protected HashSet<IPAddressRange> BlackListed;
        protected HashSet<IPAddressRange> WhiteListed;

        #region Constructor        
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionFilterMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="whiteListed">The white listed.</param>
        /// <param name="blackListed">The black listed.</param>
        public ConnectionFilterMiddleware (OwinMiddleware next, HashSet<IPAddressRange> whiteListed, HashSet<IPAddressRange> blackListed)
            : base(next)
        {
            WhiteListed = whiteListed;
            BlackListed = blackListed;
        }
        #endregion

        /// <summary>
        /// Invokes the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override async Task Invoke (IOwinContext context)
        {
            OnCheckConnection(context);

            await Next.Invoke(context);
        }

        /// <summary>
        /// Called when check connection takes place.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <exception cref="System.Web.Http.HttpResponseException"></exception>
        /// <exception cref="HttpResponseMessage"></exception>
        /// <exception cref="StringContent">Invalid Address</exception>
        protected virtual void OnCheckConnection (IOwinContext context)
        {
            var remoteAddress = context.Request.RemoteIpAddress;

            if (string.IsNullOrWhiteSpace(remoteAddress) || !IsAcceptableAddress(remoteAddress))
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Forbidden) { Content = new StringContent("Invalid Address") });
            }
        }

        /// <summary>
        /// Helper method to check, if address is explicitly white listed or
        /// not implicitly black listed.
        /// </summary>
        /// <param name="remoteAddress">The remote address.</param>
        /// <returns></returns>
        protected virtual bool IsAcceptableAddress (string remoteAddress)
        {
            IPAddressRange address;

            if (!IPAddressRange.TryParse(remoteAddress, out address))
                return false;

            var isAcceptable = WhiteListed?.Contains(address);

            if (!(isAcceptable ?? false))
                isAcceptable = !BlackListed?.Contains(address);

            return isAcceptable ?? true;
        }
    }
}
