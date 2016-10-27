namespace SimplerScore.Validators
{
    using Models;

    internal class CurrentProviderWithoutScoringModelValidatorBuilder : IValidatorBuilder<ICurrentProvider>
    {
        public IValidatorChain<ICurrentProvider> Build ()
        {
            var chain = new CurrentProviderValidator.MustHaveCurrentMeetValidator();

            chain
                .AddToChain(new CurrentProviderValidator.MustHaveCurrentEventValidator())
                .AddToChain(new CurrentProviderValidator.ShouldNotHaveScoringModelValidator());

            return chain;
        }
    }
}