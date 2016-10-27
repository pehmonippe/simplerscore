namespace SimplerScore.Models.Factories
{
    using DataAccess;
    using JetBrains.Annotations;

    /// <summary>
    /// Public interface for ioc registering.
    /// </summary>
    public interface IModelFactory
    {
    }

    public interface IModelFactory<in T> : IModelFactory
    {
        IModel Create ([NotNull] IDataProvider provider, [NotNull] IModelFactoryContainer modelFactoryContainer);

        IModel Create ([NotNull] T obj, [NotNull] IDataProvider provider, [NotNull] IModelFactoryContainer modelFactoryContainer);
    }
}
