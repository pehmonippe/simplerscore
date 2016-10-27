namespace SimplerScore.Models
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Computation;
    using DataObjects;
    using JetBrains.Annotations;

    internal static class ScoreModelExtensions
    {
        public static ReadOnlyDictionary<int, ReadOnlyCollection<int>> AsReadOnly ([NotNull][ItemNotNull] this ConcurrentDictionary<int, List<int>> deductions)
        {
            var source = deductions
                            .ToDictionary(row => row.Key, row => row.Value.AsReadOnly());

            var readOnlyDictionary = new ReadOnlyDictionary<int, ReadOnlyCollection<int>>(source);
            return readOnlyDictionary;
        }
    }

    public class ScoreModel
    {
        private readonly IComputationStrategy strategy;
        private readonly ConcurrentDictionary<int, Execution> deductions;

        public int Time
        {
            get;
            set;
        }

        public int Difficulty
        {
            get;
            set;
        }

        public int Penalty
        {
            get;
            set;
        }

        public List<Execution> Executions
        {
            get { return deductions.Values.ToList(); }
        }

        public ScoreModel ([NotNull] IComputationStrategy strategy, int judgeCount, int skillCount)
        {
            this.strategy = strategy;

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

        public Routine ComputeRoutineScore ()
        {
            var routine = new Routine
            {
                Difficulty = Difficulty,
                FlightTime = Time,
                Execution = Executions,
                Score = ComputeScore()
            };

            return routine;
        }

        private decimal ComputeScore ()
        {
            var score = strategy.ComputeScore(Executions, Time, Difficulty, Penalty);
            return score;
        }
    }
}