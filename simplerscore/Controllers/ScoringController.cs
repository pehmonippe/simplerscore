namespace SimplerScore.Controllers
{
    using Attributes;
    using Extensions;
    using JetBrains.Annotations;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DataObjects;
    using Validators;

    internal static class ScoreExtensions
    {
        private const int JudgeDimension = 0;
        private const int ExerciseDimension = 1;

        public static int Median (this IEnumerable<int> array)
        {
            var list = array
                .OrderBy(i => i)
                .ToList();

            var useAverage = 0 == list.Count % 2;
            int median;

            if (useAverage)
            {
                // this is for the arithmetic correction, but actually
                // should never occur...
                var lowBound = list.Count / 2;
                median = (list[lowBound] + list[lowBound]) / 2;
            }
            else
            {
                // take middle value of an ordered list.
                var middle = (list.Count / 2) + 1;
                median = list[middle];
            }

            return median;
        }

        public static int[] Median (this int[,] array)
        {
            var exercises = array.GetUpperBound(ExerciseDimension);
            var medians = new int[exercises + 1];

            // compute median of each exercise 
            for (var i = 0; i <= exercises; i++)
            {
                medians[i] = CollectScoresForExercises(array, i).Median();
            }

            return medians;
        }

        public static decimal Result (this IEnumerable<int> deductions)
        {
            var sum = deductions.Sum(d => d);
            var result = 10.0 - (sum / 10.0);

            return (decimal) result;
        }

        public static decimal[] Results (this int[,] array)
        {
            var judges = array.GetUpperBound(JudgeDimension);
            var results = new decimal[judges + 1];

            for (var j = 0; j <= judges; j++)
            {
                results[j] = CollectScoresForJudges(array, j).Result();                
            }

            return results;
        }

        private static IEnumerable<int> CollectScoresForJudges (int[,] array, int j)
        {
            var exercises = array.GetUpperBound(ExerciseDimension);
            var resultList = new List<int>();

            for (var e = 0; e <= exercises; e++)
            {
                resultList.Add(array[j, e]);
            }

            return resultList;
        }

        private static IEnumerable<int> CollectScoresForExercises (int[,] array, int i)
        {
            var judges = array.GetUpperBound(JudgeDimension);

            var exerciseScores = new List<int>();

            for (var j = 0; j <= judges; j++)
            {
                exerciseScores.Add(array[j, i]);
            }

            return exerciseScores;
        }
    }

    [RoutePrefix("score")]
    public class ScoringController : BaseController
    {
        [NotNull]
        private readonly ICurrentProvider current;

        public ScoringController ([NotNull] IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            current = ServiceProvider.GetService<ICurrentProvider>();
        }

        [AuthorizeAction(Action = AuthorizedAction.Scoring.Preview)]
        [HttpGet]
        [Route("details")]
        public Task<ProposedScoring> GetScoreDetails ()
        {
            var model = current.CurrentScore;

            var proposed = new ProposedScoring
            {
                Executions = model.Executions,
                Time = model.Time,
                Difficulty = model.Difficulty,
                Penalty = model.Penalty
            };
            
            return Task.FromResult(proposed);
        }

        [AuthorizeAction(Action = AuthorizedAction.Scoring.Execution)]
        [HttpGet]
        [Route("{judge:int:range(1,5)}/{skill:int:range(1,10)}/{deduction:int:range(0,5)}")]
        public IHttpActionResult SetScore ([FromUri] int judge, [FromUri] int skill, [FromUri] int deduction)
        {
            current.CurrentScore.SetSkillDeduction(judge, skill, deduction);
            return Ok();
        }

        [AuthorizeAction(Action = AuthorizedAction.Scoring.Execution)]
        [HttpGet]
        [Route("{judge:int:range(1,5)}/landing={landing:int:range(0,5)}")]
        public IHttpActionResult SetLanding ([FromUri] int judge, [FromUri] int landing)
        {
            current.CurrentScore.SetLandingDeduction(judge, landing);
            return Ok();
        }

        [AuthorizeAction(Action = AuthorizedAction.Scoring.ElementCount)]
        [HttpGet]
        [Route("skills={skills:int:range(0,10)}")]
        public IHttpActionResult SetNumberOfElementsCompleted ([FromUri] int skills = 10)
        {
            ValidateWith<CurrentProviderWithoutScoringModelValidatorBuilder>(current);

            var strategy = current.CurrentEvent.GetComputationStrategy();
            var judges = current.CurrentEvent.GetJudgeCount();

            current.CurrentScore = new ScoreModel(strategy, judges, skills);
            return Ok();
        }

        [AuthorizeAction(Action = AuthorizedAction.Scoring.Time)]
        [HttpGet]
        [Route("time={time:int:range(0,30000)}")]
        public IHttpActionResult SetFlightTime ([FromUri] int time)
        {
            // flight time should be converted to decimal
            // 1000 --> 1.0;
            var value = time / 1000.0m;

            return Ok();
        }

        [AuthorizeAction(Action = AuthorizedAction.Scoring.Execution)]
        [HttpGet]
        [Route("penalty/{judge:int:range(0,4)/{penalty:int:range(0,10)}")]
        public IHttpActionResult SetAdditionalDeduction ([FromUri] int judge, [FromUri] int penalty)
        {
            // penalty should be converted to decimal
            // 1 --> 0.1
            var value = penalty / 10.0m;

            return Ok();
        }

        /// <summary>
        /// Release results for publishing (for scoreboard).
        /// </summary>
        /// <returns></returns>
        [AuthorizeAction(Action = AuthorizedAction.Scoring.SignOff)]
        [HttpGet]
        [Route("signoff")]
        public IHttpActionResult SignOff ()
        {
            var routine = current.CurrentScore.ComputeRoutineScore();
            current.CurrentAthlete.AddRoutine(routine);

            // persist data
            Update(current.CurrentAthlete.Id, current.CurrentAthlete).Wait();

            // prepare for next
            current.CurrentScore = null;
            return Ok();
        }

        private static void ValidateWith<TValidatorBuilder> (ICurrentProvider curr)
            where TValidatorBuilder : IValidatorBuilder<ICurrentProvider>, new()
        {
            var builder = new TValidatorBuilder();
            var validator = builder.Build();

            validator.Validate(curr);
        }
    }
}
