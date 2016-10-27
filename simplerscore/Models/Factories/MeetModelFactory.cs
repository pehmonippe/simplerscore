namespace SimplerScore.Models.Factories
{
    using DataAccess;
    using DataObjects;

    internal class MeetModelFactory : IModelFactory<Meet>
    {
        public IModel Create (IDataProvider provider, IModelFactoryContainer modelFactoryContainer)
        {
            var model = new MeetModel(provider, modelFactoryContainer);
            return model;
        }

        public IModel Create (Meet meet, IDataProvider provider, IModelFactoryContainer modelFactoryContainer)
        {
            var model = new MeetModel(meet, provider, modelFactoryContainer);
            return model;
        }
    }
}