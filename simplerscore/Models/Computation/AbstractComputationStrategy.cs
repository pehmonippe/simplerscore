namespace SimplerScore.Models.Computation
{
    using DataObjects;
    using JetBrains.Annotations;
    using System.Collections.Generic;

    internal abstract class AbstractComputationStrategy : IComputationStrategy
    {
        protected readonly IComputer Computer;

        protected AbstractComputationStrategy ([CanBeNull] IComputer computer)
        {
            Computer = computer;
        }

        /// <summary>
        /// Computes the score.
        /// </summary>
        /// <param name="executions">The executions.</param>
        /// <param name="time">The time.</param>
        /// <param name="difficulty">The difficulty.</param>
        /// <param name="penalty">The penalty.</param>
        /// <returns></returns>
        public decimal ComputeScore ([ItemNotNull] IEnumerable<Execution> executions, int time, int difficulty, int penalty)
        {
            // execution score is x10
            // penalty is x10
            // difficulty is x10
            // time is x1000 (including 1/1000 fractions)
            var score = 100 * (OnComputeScore(executions) + difficulty - penalty) + time;
            return score / 1000m;
        }

        /// <summary>
        /// Derived classes override this method to implement strategy specific computation algorithm.
        /// </summary>
        /// <param name="executions">The executions.</param>
        /// <returns></returns>
        protected abstract decimal OnComputeScore ([NotNull, ItemNotNull] IEnumerable<Execution> executions);
    }
}