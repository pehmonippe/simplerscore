namespace SimplerScore.Models.Computation
{
    using System.Collections.Generic;
    using System.Linq;
    using DataObjects;
    using JetBrains.Annotations;

    internal class TotalDeductionComputationStrategy : AbstractComputationStrategy
    {
        public TotalDeductionComputationStrategy ([CanBeNull] IComputer computer) 
            : base(computer)
        {
        }

        protected override decimal OnComputeScore ([ItemNotNull] IEnumerable<Execution> executions)
        {
            // compute per judge deductions
            var perJudge = JudgeDeductions(executions);
            var score = ComputeScore(perJudge);

            return score;
        }

        protected virtual List<int> JudgeDeductions ([NotNull] [ItemNotNull] IEnumerable<Execution> executions)
        {
            // compute per judge score
            var scores = executions
                .Select(ComputeJudgeScore)
                .ToList();

            return scores;
        }

        protected virtual int ComputeJudgeScore ([NotNull] [ItemNotNull] Execution execution)
        {
            var score = execution.Elements
                .Sum(skill => skill);

            var judgeScore = 10 * execution.Elements.Count - score;
            return judgeScore;
        }

        protected virtual decimal ComputeScore ([NotNull] [ItemNotNull] List<int> scores)
        {
            var sc = scores
                .Select(s => (decimal) s)
                .ToList();

            if (4 == sc.Count)
            {
                // missing judge is average of given points
                var missingScore = sc.Average();
                sc.Add(missingScore);
            }

            // remove highest and lowest score
            var score = scores
                .OrderBy(s => s)
                .Skip(1)
                .Take(3)
                .Sum();

            return score;
        }
    }
}