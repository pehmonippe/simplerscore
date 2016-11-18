namespace SimplerScore.Models.Factories
{
    using DataAccess;
    using DataObjects;

    internal class TimePointModelFactory : IModelFactory<TimePoint>
    {
        public IModel Create (IDataProvider provider, IModelFactoryContainer modelFactoryContainer)
        {
            var model = new TimePointModel(provider, modelFactoryContainer);
            return model;
        }

        public IModel Create (TimePoint timePoint, IDataProvider provider, IModelFactoryContainer modelFactoryContainer)
        {
            var model = new TimePointModel(timePoint, provider, modelFactoryContainer);
            return model;
        }
    }
}