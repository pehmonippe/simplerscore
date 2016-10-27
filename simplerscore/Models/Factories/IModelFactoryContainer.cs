namespace SimplerScore.Models.Factories
{
    using DataAccess;
    using JetBrains.Annotations;

    public interface IModelFactoryContainer
    {
        IModel Create<T> ([NotNull] IDataProvider provider);

        IModel Create<T> ([NotNull] T entity, [NotNull] IDataProvider provider);

        IModelFactory<T> ModelFactoryOfType<T> (bool throwIfMissing = true);
    }
}