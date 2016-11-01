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

        public void Validate (ICurrentProvider curr, object obj = null)
        {
            OnValidate(curr, obj);
            Next?.Validate(curr, obj);
        }

        public IValidatorChain<ICurrentProvider> AddToChain (IValidatorChain<ICurrentProvider> next)
        {
            Next = next;
            return next;
        }

        protected abstract void OnValidate (ICurrentProvider curr, object obj);

        internal class MustHaveCurrentEventValidator : CurrentProviderValidator
        {
            protected override void OnValidate (ICurrentProvider curr, object obj)
            {
                if (null == curr.CurrentEvent)
                    throw new NoActiveEventException();
            }
        }

        internal class MustHaveCurrentMeetValidator : CurrentProviderValidator
        {
            protected override void OnValidate (ICurrentProvider curr, object obj)
            {
                if (null == curr.CurrentMeet)
                    throw new NoActiveMeetException();
            }
        }

        internal class MustHaveScoringModelValidator : CurrentProviderValidator
        {
            protected override void OnValidate (ICurrentProvider curr, object obj)
            {
                if (null == curr.CurrentScore)
                    throw new ScoringModelAlreadyExistsException();
            }
        }

        internal class ShouldNotHaveScoringModelValidator : CurrentProviderValidator
        {
            protected override void OnValidate (ICurrentProvider curr, object obj)
            {
                if (null != curr.CurrentScore)
                    throw new ScoringModelAlreadyExistsException();
            }
        }

        internal class SkillMustBeLessThanCompletedElementsValidator : CurrentProviderValidator
        {
            protected override void OnValidate (ICurrentProvider curr, object obj)
            {
                if (!(obj is int))      // do not perform validation, if unknonw type
                    return;

                var skill = (int)obj;
                var completedElementCount = curr.CurrentScore.CompletedElements;

                if (skill >= completedElementCount)
                    throw new SkillOutOfCompletedRangeException(skill, completedElementCount);
            }
        }
    }
}
