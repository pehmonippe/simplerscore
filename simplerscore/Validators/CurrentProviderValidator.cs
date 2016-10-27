namespace SimplerScore.Validators
{
    using Exceptions;
    using Models;

    internal abstract class CurrentProviderValidator : IValidatorChain<ICurrentProvider>
    {
        public IValidatorChain<ICurrentProvider> Next
        {
            get;
            private set;
        }

        public void Validate (ICurrentProvider curr)
        {
            OnValidate(curr);
            Next?.Validate(curr);
        }

        public IValidatorChain<ICurrentProvider> AddToChain (IValidatorChain<ICurrentProvider> next)
        {
            Next = next;
            return next;
        }

        protected abstract void OnValidate (ICurrentProvider curr);

        internal class MustHaveCurrentEventValidator : CurrentProviderValidator
        {
            protected override void OnValidate (ICurrentProvider curr)
            {
                if (null == curr.CurrentEvent)
                    throw new NoActiveEventException();
            }
        }

        internal class MustHaveCurrentMeetValidator : CurrentProviderValidator
        {
            protected override void OnValidate (ICurrentProvider curr)
            {
                if (null == curr.CurrentMeet)
                    throw new NoActiveMeetException();
            }
        }

        internal class MustHaveScoringModelValidator : CurrentProviderValidator
        {
            protected override void OnValidate (ICurrentProvider curr)
            {
                if (null == curr.CurrentScore)
                    throw new ScoringModelAlreadyExistsException();
            }
        }

        internal class ShouldNotHaveScoringModelValidator : CurrentProviderValidator
        {
            protected override void OnValidate (ICurrentProvider curr)
            {
                if (null != curr.CurrentScore)
                    throw new ScoringModelAlreadyExistsException();
            }
        }
    }
}