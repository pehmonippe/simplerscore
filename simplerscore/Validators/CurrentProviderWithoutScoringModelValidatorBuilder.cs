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

    internal class CurrentProviderWithScoringModelValidatorBuilder : IValidatorBuilder<ICurrentProvider>
    {
        public IValidatorChain<ICurrentProvider> Build ()
        {
            var chain = new CurrentProviderValidator.MustHaveCurrentMeetValidator();

            chain
                .AddToChain(new CurrentProviderValidator.MustHaveCurrentEventValidator())
                .AddToChain(new CurrentProviderValidator.MustHaveScoringModelValidator());

            return chain;
        }
    }

    internal class CurrentProviderWithSkillValidatorBuilder : IValidatorBuilder<ICurrentProvider>
    {
        public IValidatorChain<ICurrentProvider> Build ()
        {
            var chain = new CurrentProviderValidator.MustHaveCurrentMeetValidator();

            chain
                .AddToChain(new CurrentProviderValidator.MustHaveCurrentEventValidator())
                .AddToChain(new CurrentProviderValidator.MustHaveScoringModelValidator())
                .AddToChain(new CurrentProviderValidator.SkillMustBeLessThanCompletedElementsValidator());

            return chain;
        }
    }
}
