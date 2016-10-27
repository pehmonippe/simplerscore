namespace SimplerScore.Models.Computation
{
    using System;

    public interface IComputationStrategyFactory
    {
        IComputationStrategy Create (ComputationStrategy strategy);
    }

    public class ComputationStrategyFactory : IComputationStrategyFactory
    {
        public IComputationStrategy Create (ComputationStrategy strategy)
        {
            IComputationStrategy instance;

            switch (strategy)
            {
                case ComputationStrategy.TotalDeduction:
                    instance = new TotalDeductionComputationStrategy();
                    break;

                case ComputationStrategy.MedianDeduction:
                    instance = new MedianDeductionComputationStrategy();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return instance;
        }
    }
}