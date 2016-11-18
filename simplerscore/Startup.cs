namespace SimplerScore
{
    using Authorization;
    using ExceptionHandlers;
    using Extensions;
    using JetBrains.Annotations;
    using Microsoft.Owin;
    using Microsoft.Owin.Hosting;
    using Microsoft.Owin.Security.OAuth;
    using Microsoft.Practices.Unity;
    using Owin;
    using System;
    using System.Web.Http;
    using System.Web.Http.ExceptionHandling;
    using System.Web.Http.ModelBinding;
    using System.Web.Http.ModelBinding.Binders;
    using Controllers.ModelBinders;
    using DataObjects;
    using Microsoft.Practices.Unity.Configuration;

    public class Startup
    {
        #region Public methods        
        /// <summary>
        /// Starts the web application host.
        /// </summary>
        /// <param name="baseAddress">The base address.</param>
        public void StartWebAppHost ([NotNull] string baseAddress)
        {
            WebApp.Start(baseAddress, Configuration);
        }

        /// <summary>
        /// Configurations the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <remarks>
        /// This method must be public in order to have
        /// it discoverable by WebApi.
        /// </remarks>
        public virtual void Configuration ([NotNull] IAppBuilder app)
        {
            // generic setup            
            ConfigureLogging(app);

            ConfigureErrorHandling(app);

            ConfigureAuthentication(app);
            ConfigureAuthorization(app);

            // api configuration
            ConfigureWebApi(app);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Configures the web API.
        /// </summary>
        /// <param name="app">The application.</param>
        protected virtual void ConfigureWebApi ([NotNull] IAppBuilder app)
        {
            var config = new HttpConfiguration();

            // enable attribute routing
            config.MapHttpAttributeRoutes(new CustomDirectRouteProvider("v1"));

            // limit controller search to running one.
            //config.Services.Replace(typeof(IAssembliesResolver), new LocalAssembliesResolver());
            config.Services.Replace(typeof(IExceptionHandler), new PassthroughExceptionHandler());

            var container = new UnityContainer();
            container.LoadConfiguration();
            config.DependencyResolver = new UnityResolver(container);

            var provider = new SimpleModelBinderProvider(typeof (TimePoint), new TimePointModelBinder());
            config.Services.Insert(typeof (ModelBinderProvider), 0, provider);

            app.UseWebApi(config);
            //config.EnsureInitialized();
        }

        /// <summary>
        /// Configures the logging.
        /// </summary>
        /// <param name="app">The application.</param>
        protected virtual void ConfigureLogging ([NotNull] IAppBuilder app)
        {
            app.UseLoggingMiddleware();
        }

        /// <summary>
        /// Configures the error handling.
        /// </summary>
        /// <param name="app">The application.</param>
        protected virtual void ConfigureErrorHandling ([NotNull] IAppBuilder app)
        {
            app.UseErrorHandlingMiddleware();
        }

        /// <summary>
        /// Configures the authentication.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <remarks>See parameterizing from http://bitoftech.net/wp-content/uploads/2014/05/TokenPostRequest.png</remarks>
        protected virtual void ConfigureAuthentication ([NotNull] IAppBuilder app)
        {
            var oAuthOptions = new OAuthAuthorizationServerOptions
            {
                TokenEndpointPath = new PathString("/Token"),
                Provider = new ApplicationOAuthServerProvider(),
                AccessTokenExpireTimeSpan = TimeSpan.FromDays(1),

                AllowInsecureHttp = true
            };

            app.UseOAuthAuthorizationServer(oAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }

        /// <summary>
        /// Configures the authorization.
        /// </summary>
        /// <param name="app">The application.</param>
        protected virtual void ConfigureAuthorization ([NotNull] IAppBuilder app)
        {
            app.UseAuthorizationMiddleware();
        }

        #endregion
    }
}
