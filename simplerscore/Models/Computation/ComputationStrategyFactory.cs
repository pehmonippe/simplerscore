namespace SimplerScore.Models.Computation
{
    using System;
    using JetBrains.Annotations;

    public interface IComputationStrategyFactory
    {
        IComputationStrategy Create (ComputationStrategy strategy);
    }

    public class ComputationStrategyFactory : IComputationStrategyFactory
    {
        private readonly IComputer computer;

        public ComputationStrategyFactory ([NotNull] IComputer computer)
        {
            this.computer = computer;
        }

        public IComputationStrategy Create (ComputationStrategy strategy)
        {
            IComputationStrategy instance;

            switch (strategy)
            {
                case ComputationStrategy.TotalDeduction:
                    instance = new TotalDeductionComputationStrategy(computer);
                    break;

                case ComputationStrategy.MedianDeduction:
                    instance = new MedianDeductionComputationStrategy(computer);
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null);
            }

            return instance;
        }
    }
}