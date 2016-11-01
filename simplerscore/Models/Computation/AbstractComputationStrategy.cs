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
            var score = OnComputeScore(executions) + time + difficulty - penalty;
            return score;
        }

        /// <summary>
        /// Derived classes override this method to implement strategy specific computation algorithm.
        /// </summary>
        /// <param name="executions">The executions.</param>
        /// <returns></returns>
        protected abstract decimal OnComputeScore ([NotNull, ItemNotNull] IEnumerable<Execution> executions);
    }
}