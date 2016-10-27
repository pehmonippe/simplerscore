namespace SimplerScore.Validators
{
    using Models;

    internal class CurrentProviderWithCurrentEventValidatorBuilder : IValidatorBuilder<ICurrentProvider>
    {
        public IValidatorChain<ICurrentProvider> Build ()
        {
            var chain = new CurrentProviderValidator.MustHaveCurrentMeetValidator();

            chain
                .AddToChain(new CurrentProviderValidator.MustHaveCurrentEventValidator());

            return chain;
        }
    }
}