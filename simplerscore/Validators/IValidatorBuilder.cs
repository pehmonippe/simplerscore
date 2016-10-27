namespace SimplerScore.Validators
{

    public interface IValidatorBuilder<T>
    {
        IValidatorChain<T> Build ();
    }
}