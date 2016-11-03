namespace SimplerScore.Models
{
    using System.Collections.Concurrent;
    using System.Linq;
    using Computation;
    using DataObjects;
    using JetBrains.Annotations;

    public class ScoreModel : Routine, IModel
    {
        private readonly IComputationStrategy strategy;
        private readonly ConcurrentDictionary<int, Execution> deductions;

        public int CompletedElements
        {
            get;
        }

        public ScoreModel ([NotNull] IComputationStrategy strategy, int judgeCount, int skillCount)
        {
            this.strategy = strategy;
            CompletedElements = skillCount;

            deductions = new ConcurrentDictionary<int, Execution>();
            for (var j = 0; j < judgeCount; j++)
            {
                deductions.TryAdd(j, new Execution(skillCount));
            }
        }

        public bool SetSkillDeduction (int judge, int element, int deduction)
        {
            Execution execution;

            if (!deductions.TryGetValue(judge, out execution))
                return false;

            if (element < 0 || element >= execution.Elements.Count)
                return false;

            execution.Elements[element] = deduction;
            return true;
        }

        public bool SetLandingDeduction (int judge, int landing)
        {
            Execution execution;

            if (!deductions.TryGetValue(judge, out execution))
                return false;

            execution.Landing = landing;
            return true;
        }

        public bool SetAdditionalDeduction (int judge, int additional)
        {
            Execution execution;

            if (!deductions.TryGetValue(judge, out execution))
                return false;

            execution.Additional = additional;
            return true;
        }

        public Routine ComputeRoutineScore ()
        {
            var routine = new Routine
            {
                Difficulty = Difficulty,
                Time = Time,
                Executions = deductions.Values.ToList(),
                Score = ComputeScore()
            };

            return routine;
        }

        private decimal ComputeScore ()
        {
            var score = strategy.ComputeScore(deductions.Values.ToList(), Time, Difficulty, Penalty);
            return score;
        }
    }
}
