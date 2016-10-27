namespace SimplerScore.Models.Factories
{
    using DataAccess;
    using DataObjects;

    internal class JudgesPanelModelFactory : IModelFactory<JudgesPanel>
    {
        public IModel Create (IDataProvider provider, IModelFactoryContainer modelFactoryContainer)
        {
            var model = new JudgesPanelModel();
            return model;
        }

        public IModel Create (JudgesPanel obj, IDataProvider provider, IModelFactoryContainer modelFactoryContainer)
        {
            var model = new JudgesPanelModel(obj);
            return model;
        }
    }
}
