namespace SimplerScore.Extensions
{
    using System;

    public static class ServiceProviderExtensions
    {
        public static TService GetService<TService> (this IServiceProvider serviceProvider)
        {
            var service = (TService) serviceProvider.GetService(typeof (TService));
            return service;
        }
    }
}
