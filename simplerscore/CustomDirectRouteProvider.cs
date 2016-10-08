namespace SimplerScore
{
    using System.Collections.Generic;
    using System.Web.Http.Controllers;
    using System.Web.Http.Routing;

    /// <summary>
    /// CustomDirectRouteProvider allows usage of inherited route attributes - 
    /// within i.e. generics based controllers - and global prefix applied to all apis.
    /// 
    /// inheritance from:
    /// http://stackoverflow.com/questions/19989023/net-webapi-attribute-routing-and-inheritance
    /// 
    /// global prefix from:
    /// http://www.strathweb.com/2015/10/global-route-prefixes-with-attribute-routing-in-asp-net-web-api/
    /// </summary>
    public class CustomDirectRouteProvider : DefaultDirectRouteProvider
    {
        private readonly string globalPrefix;

        public CustomDirectRouteProvider (string globalPrefix)
        {
            this.globalPrefix = globalPrefix;
        }

        protected override string GetRoutePrefix (HttpControllerDescriptor controllerDescriptor)
        {
            var existingPrefix = base.GetRoutePrefix(controllerDescriptor);

            if (string.IsNullOrWhiteSpace(existingPrefix))
                return globalPrefix;

            // ReSharper disable once UseStringInterpolation
            var appendedPrefix = $"{globalPrefix}/{existingPrefix}";
            return appendedPrefix;
        }

        protected override IReadOnlyList<IDirectRouteFactory> GetActionRouteFactories (HttpActionDescriptor actionDescriptor)
        {
            // inherit route attributes decorated on base class controller's actions
            var list = actionDescriptor.GetCustomAttributes<IDirectRouteFactory>(true);
            return list;
        }
    }
}