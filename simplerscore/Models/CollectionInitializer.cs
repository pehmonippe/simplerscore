namespace SimplerScore.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using DataAccess;
    using Factories;
    using JetBrains.Annotations;

    internal class CollectionInitializer
    {
        private readonly IDataProvider provider;
        private readonly IModelFactoryContainer modelFactoryContainer;

        public CollectionInitializer ([NotNull] IDataProvider provider, [NotNull] IModelFactoryContainer modelFactoryContainer)
        {
            this.provider = provider;
            this.modelFactoryContainer = modelFactoryContainer;
        }

        public List<TModel> Initialize<T, TModel> (Expression<Func<T, bool>> criteria)
            where T : class, new()
        {
            if (null == provider)
                return new List<TModel>();

            var factory = modelFactoryContainer.ModelFactoryOfType<T>();

            var collection = provider.Collection<T>()
                .Find(criteria)
                .ToList()
                .ConvertAll(t => (TModel)factory.Create(t, provider, modelFactoryContainer));

            return collection;
        }
    }
}