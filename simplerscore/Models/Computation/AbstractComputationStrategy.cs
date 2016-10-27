namespace SimplerScore.Models.Computation
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using System.Linq;
    using DataObjects;

    internal abstract class AbstractComputationStrategy : IComputationStrategy
    {
        /// <summary>
        /// Computes the score.
        /// </summary>
        /// <param name="deductions">The deductions.</param>
        /// <param name="time">The time.</param>
        /// <param name="difficulty">The difficulty.</param>
        /// <param name="penalty">The penalty.</param>
        /// <returns></returns>
        public decimal ComputeScore ([ItemNotNull] IEnumerable<Execution> deductions, int time, int difficulty, int penalty)
        {
            var executions = deductions
                .Select(judge => judge.Elements.ToList())
                .ToList();

            //TODO: check this computation algorithm
            var score = OnComputeScore(executions) + time + difficulty - penalty;
            return score;
        }

        /// <summary>
        /// Derived classes override this method to implement strategy specific computation algorithm.
        /// </summary>
        /// <param name="deductions">The deductions.</param>
        /// <returns></returns>
        protected abstract decimal OnComputeScore ([NotNull, ItemNotNull] List<List<int>> deductions);
    }
}