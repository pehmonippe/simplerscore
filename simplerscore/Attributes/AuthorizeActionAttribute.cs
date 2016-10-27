namespace SimplerScore.Attributes
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
    public class AuthorizeActionAttribute : AuthorizeAttribute
    {
        public string Action
        {
            get;
            set;
        }
        public override void OnAuthorization (HttpActionContext actionContext)
        {
            base.OnAuthorization(actionContext);
        }

        public override Task OnAuthorizationAsync (HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            return base.OnAuthorizationAsync(actionContext, cancellationToken);
        }
    }
}
