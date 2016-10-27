namespace SimplerScore.Models.Factories
{
    using DataAccess;
    using DataObjects;

    internal class AthleteModelFactory : IModelFactory<Athlete>
    {
        public IModel Create (IDataProvider provider, IModelFactoryContainer modelFactoryContainer)
        {
            var model = new AthleteModel();
            return model;
        }

        public IModel Create (Athlete obj, IDataProvider provider, IModelFactoryContainer modelFactoryContainer)
        {
            var model = new AthleteModel(obj);
            return model;
        }
    }
}
