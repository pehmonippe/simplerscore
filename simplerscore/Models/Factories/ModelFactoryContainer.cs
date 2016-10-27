namespace SimplerScore.Models.Factories
{
    using System;
    using System.Linq;
    using DataAccess;
    using JetBrains.Annotations;

    public class ModelFactoryContainer : IModelFactoryContainer
    {
        /// <summary>
        /// The model factories instance.
        /// </summary>
        private readonly IModelFactory[] modelFactories;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelFactoryContainer"/> class.
        /// </summary>
        /// <param name="modelFactories">The model factories.</param>
        public ModelFactoryContainer ([CanBeNull] IModelFactory[] modelFactories)
        {
            this.modelFactories = modelFactories;
        }

        /// <summary>
        /// Creates the model for specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public IModel Create<T> (IDataProvider provider)
        {
            var factory = ModelFactoryOfType<T>();

            var model = factory?.Create(provider, this);
            return model;
        }

        /// <summary>
        /// Creates the model for the root entity.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity">The entity.</param>
        /// <param name="provider">The provider.</param>
        /// <returns></returns>
        public IModel Create<T> (T entity, IDataProvider provider)
        {
            var factory = ModelFactoryOfType<T>();

            var model = factory?.Create(entity, provider, this);
            return model;
        }

        public IModelFactory<T> ModelFactoryOfType<T> (bool throwIfMissing = true)
        {
            var factory = modelFactories
                .OfType<IModelFactory<T>>()
                .FirstOrDefault();

            if (throwIfMissing && null == factory)
                throw new ArgumentNullException(nameof(factory), $"Missing model factory for type {typeof (T).Name}.");

            return factory;
        }
    }
}