namespace SimplerScore.Models.Factories
{
    using Computation;
    using DataAccess;
    using DataObjects;
    using JetBrains.Annotations;

    internal class EventModelFactory : IModelFactory<Event>
    {
        private readonly IComputationStrategyFactory strategyFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventModelFactory"/> class.
        /// </summary>
        /// <param name="strategyFactory">The strategy factory.</param>
        public EventModelFactory ([NotNull] IComputationStrategyFactory strategyFactory)
        {
            this.strategyFactory = strategyFactory;
        }

        /// <summary>
        /// Creates the specified model without root element.
        /// </summary>
        /// <param name="provider">The provider.</param>
        /// <param name="modelFactoryContainer">The model factory container.</param>
        /// <returns></returns>
        public IModel Create (IDataProvider provider, IModelFactoryContainer modelFactoryContainer)
        {
            var model = new EventModel(provider, strategyFactory, modelFactoryContainer);
            return model;
        }

        /// <summary>
        /// Creates the specified model using root element.
        /// </summary>
        /// <param name="e">The e.</param>
        /// <param name="provider">The provider.</param>
        /// <param name="modelFactoryContainer">The model factory container.</param>
        /// <returns></returns>
        public IModel Create (Event e, IDataProvider provider, IModelFactoryContainer modelFactoryContainer)
        {
            var model = new EventModel(provider, strategyFactory, e, modelFactoryContainer);
            return model;
        }
    }
}
