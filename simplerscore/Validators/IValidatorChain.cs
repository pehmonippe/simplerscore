namespace SimplerScore.Validators
{
    public interface IValidatorChain<T>
    {
        void Validate (T obj);

        IValidatorChain<T> Next
        {
            get;
        }

        IValidatorChain<T> AddToChain (IValidatorChain<T> next);
    }
}