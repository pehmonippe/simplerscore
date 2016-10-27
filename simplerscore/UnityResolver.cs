namespace SimplerScore
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Dependencies;
    using JetBrains.Annotations;
    using Microsoft.Practices.Unity;

    public class UnityResolver : IDependencyResolver
    {
        private readonly IUnityContainer container;

        public UnityResolver (IUnityContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            this.container = container;
        }

        [CanBeNull]
        public object GetService (Type serviceType)
        {
            try
            {
                return container.Resolve(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return null;
            }
        }

        [NotNull]
        public IEnumerable<object> GetServices (Type serviceType)
        {
            try
            {
                return container.ResolveAll(serviceType);
            }
            catch (ResolutionFailedException)
            {
                return new List<object>();
            }
        }

        [NotNull]
        public IDependencyScope BeginScope ()
        {
            var child = container.CreateChildContainer();
            return new UnityResolver(child);
        }

        public void Dispose ()
        {
            container.Dispose();
        }
    }
}
