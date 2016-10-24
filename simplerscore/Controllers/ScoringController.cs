namespace SimplerScore.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DataAccess;
    using JetBrains.Annotations;
    using System.Web.Http;

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

    public class ProposedScoring
    {
        public int[,] Deductions
        {
            get;
            set;
        }

        public int[] Medians
        {
            get;
            set;
        }

        public decimal[] Results
        {
            get;
            set;
        }

        public decimal Time
        {
            get;
            set;
        }

        public decimal Penalty
        {
            get;
            set;
        }
    }

    [RoutePrefix("score")]
    public class ScoringController : BaseController
    {
        public ScoringController ([NotNull] IDataProvider provider) 
            : base(provider)
        {
        }

        //[Authorize(Roles = "ChiefJudge")]
        [HttpGet]
        [Route("")]
        public Task<ProposedScoring> GetScores ()
        {
            var proposed = new ProposedScoring
            {
                Deductions = new int[6, 10]
            };

            proposed.Medians = proposed.Deductions.Median();
            proposed.Results = proposed.Deductions.Results();
            return Task.FromResult(proposed);
        }

        [Authorize(Roles ="Judge")]
        [HttpGet]
        [Route("{judge:int:range(1,5)}/{score:decimal}")]
        public IHttpActionResult SetScore ([FromUri] int judge, [FromUri] decimal score)
        {
            return Ok();
        }

        [Authorize(Roles ="Judge")]
        [HttpGet]
        [Route("{judge:int:range(1,5)}/{exercise:int:range(1,10)}/{deduction:int:range(0,5)}")]
        public IHttpActionResult SetScore ([FromUri] int judge, [FromUri] int exercise, [FromUri] int deduction)
        {
            return Ok();
        }

        [Authorize(Roles = "ChiefJudge")]
        [HttpGet]
        [Route("exercises={exercises:int:range(0,10)}")]
        public IHttpActionResult SetNumberOfExercises ([FromUri] int exercises = 10)
        {
            return Ok();
        }

        [Authorize(Roles = "FlightTimeJudge")]
        [HttpGet]
        [Route("time={time:int:range(0,30000)}")]
        public IHttpActionResult SetFlightTime ([FromUri] int time)
        {
            // flight time should be converted to decimal
            // 1000 --> 1.0;
            var value = time / (decimal) 1000.0;

            return Ok();
        }

        [Authorize(Roles = "ChiefJudge")]
        [HttpGet]
        [Route("penalty={penalty:int:range(0,100)}")]
        public IHttpActionResult SetPenalty ([FromUri] int penalty)
        {
            // penalty should be converted to decimal
            // 1 --> 0.1
            var value = penalty / (decimal) 10.0;

            return Ok();
        }

        /// <summary>
        /// Release results for publishing (for scoreboard).
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "ChiefJudge")]
        [HttpGet]
        [Route("signoff")]
        public IHttpActionResult SignOff ()
        {
            return Ok();
        }
    }
}
