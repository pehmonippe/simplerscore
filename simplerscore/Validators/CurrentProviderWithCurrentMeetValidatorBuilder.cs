namespace SimplerScore.Validators
{
    using Models;

    internal class CurrentProviderWithCurrentMeetValidatorBuilder : IValidatorBuilder<ICurrentProvider>
    {
        public IValidatorChain<ICurrentProvider> Build ()
        {
            var chain = new CurrentProviderValidator.MustHaveCurrentMeetValidator();

            return chain;
        }
    }
}