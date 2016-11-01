namespace SimplerScore.Validators
{
    using JetBrains.Annotations;

    public interface IValidatorChain<T>
    {
        void Validate ([NotNull] T obj, [CanBeNull] object userData = null);

        IValidatorChain<T> Next
        {
            get;
        }

        IValidatorChain<T> AddToChain (IValidatorChain<T> next);
    }
}