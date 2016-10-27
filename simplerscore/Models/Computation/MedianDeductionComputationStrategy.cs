namespace SimplerScore.Models.Computation
{
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;

    internal static class MedianComputationExtensions
    {
        /// <summary>
        /// Computes median of the values.
        /// </summary>
        /// <param name="execution">The execution.</param>
        /// <returns></returns>
        public static decimal Median (this List<int> execution)
        {
            var useAverage = 0 == execution.Count % 2;
            decimal median;

            if (useAverage)
            {
                var lowerMiddle = execution.Count / 2;

                median = (decimal) new List<int>
                {
                    execution[lowerMiddle],
                    execution[lowerMiddle + 1]
                }.Average();
            }
            else
            {
                var middle = execution.Count / 2 + 1;
                median = execution[middle];
            }

            return median;
        }
    }

    internal class MedianDeductionComputationStrategy : AbstractComputationStrategy
    {
        protected override decimal OnComputeScore (List<List<int>> executions)
        {
            var score = 3.0m * Medians(executions).Sum();
            return score;
        }

        /// <summary>
        /// Computes the median values for each exercise.
        /// </summary>
        /// <param name="executions">The executions.</param>
        /// <returns></returns>
        protected virtual decimal[] Medians ([NotNull, ItemNotNull] List<List<int>> executions)
        {
            var skills = executions.First().Count;
            var medians = new decimal[skills];

            for (var skill = 0; skill < skills; skill++)
            {
                // compute median of each exercise 
                medians[skill] = executions
                    .Select(execution => execution[skill])
                    .ToList()
                    .Median();
            }

            return medians;
        }
    }
}
