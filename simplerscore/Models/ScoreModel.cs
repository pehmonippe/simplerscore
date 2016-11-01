namespace SimplerScore.Models
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
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

        //TODO: get rid of the NEW override
        public new List<Execution> Executions
        {
            get { return deductions.Values.ToList(); }
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
                Executions = Executions,
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

	internal class Skill
    {
        public List<int> Deductions
        {
            get;
            set;
        }

        public Skill (List<int> deductions)
        {
            Deductions = deductions;
        }
    }

    internal class ExecutionTransposed
    {
        public List<Skill> Skills
        {
            get;
            set;
        }

        public List<int> Landings
        {
            get;
            set;
        }

        public List<int> Additionals
        {
            get;
            set;
        }
    }  
}
