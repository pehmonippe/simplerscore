namespace SimplerScore.Models.Computation
{
    using System.Collections.Generic;
    using System.Linq;
    using DataObjects;
    using JetBrains.Annotations;

    public interface IComputer
    {
        decimal Median (IEnumerable<int> items);
    }

    internal static class ComputerExtensions
    {
        public static decimal Median ([NotNull] this IEnumerable<int> items, [NotNull] IComputer computer)
        {
            var median = computer.Median(items);
            return median;
        }
    }

    internal class MedianComputer : IComputer
    {
        /// <summary>
        /// Computes aritmetic median of the values.
        /// </summary>
        /// <param name="items">The items.</param>
        /// <returns></returns>
        public decimal Median (IEnumerable<int> items)
        {
            var list = items.OrderBy(i => i).ToList();

            var useAverage = 0 == list.Count % 2;
            decimal median;

            if (useAverage)
            {
                var lowerMiddle = list.Count / 2;

                median = (decimal) new List<int>
                {
                    list[lowerMiddle],
                    list[lowerMiddle + 1]
                }.Average();
            }
            else
            {
                var middle = list.Count / 2 + 1;
                median = list[middle];
            }

            return median;
        }
    }

    internal class MedianDeductionComputationStrategy : AbstractComputationStrategy
    {
        public MedianDeductionComputationStrategy ([NotNull] IComputer computer) 
            : base(computer)
        {
        }

        protected override decimal OnComputeScore ([ItemNotNull] IEnumerable<Execution> executions)
        {
            var score = 3.0m * Medians(executions).Sum();
            return score;
        }

        /// <summary>
        /// Computes the sum of median values of elements.
        /// </summary>
        /// <param name="executions">The executions.</param>
        /// <returns></returns>
        protected virtual IList<decimal> Medians ([NotNull, ItemNotNull] IEnumerable<Execution> executions)
        {
            var skills = new SkillEnumerator(executions);

            var medians = skills
                .Select(execution => execution.Median(Computer))
                .ToList();

            return medians;
        }
    }
}
