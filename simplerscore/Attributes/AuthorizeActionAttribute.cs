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
            return;
            //base.OnAuthorization(actionContext);
        }

        public override async Task OnAuthorizationAsync (HttpActionContext actionContext, CancellationToken cancellationToken)
        {
            await Task.Yield();
            //return base.OnAuthorizationAsync(actionContext, cancellationToken);
        }
    }
}
