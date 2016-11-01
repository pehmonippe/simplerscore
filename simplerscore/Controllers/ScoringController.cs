namespace SimplerScore.Controllers
{
    using System.Linq;
    using Attributes;
    using DataAccess;
    using DataObjects;
    using JetBrains.Annotations;
    using Models;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Validators;

    [RoutePrefix("score")]
    public class ScoringController : BaseController
    {
        [NotNull]
        private readonly ICurrentProvider currentProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ScoringController"/> class.
        /// </summary>
        /// <param name="currentProvider">The current provider.</param>
        /// <param name="current">The current.</param>
        public ScoringController ([NotNull] ICurrentProvider currentProvider, [NotNull] IDataProvider current)
            : base(current)
        {
            this.currentProvider = currentProvider;
        }

        /// <summary>
        /// Sets the number of elements completed. Opens a new scoring model to currentProvider.
        /// </summary>
        /// <param name="skills">The skills.</param>
        /// <returns></returns>
        [AuthorizeAction(Action = AuthorizedAction.Scoring.ElementCount)]
        [HttpGet]
        [Route("skills={skills:int:range(0,10)}")]
        public IHttpActionResult SetNumberOfElementsCompleted ([FromUri] int skills = 10)
        {
            ValidateWith<CurrentProviderWithoutScoringModelValidatorBuilder>(currentProvider);

            var strategy = currentProvider.CurrentEvent.GetComputationStrategy();
            var judges = currentProvider.CurrentEvent.GetJudgeCount();

            currentProvider.CurrentScore = new ScoreModel(strategy, judges, skills);
            return Ok();
        }

        /// <summary>
        /// Sets execution judge's deduction of a specified skill.
        /// </summary>
        /// <param name="judge">The judge.</param>
        /// <param name="skill">The skill.</param>
        /// <param name="deduction">The deduction.</param>
        /// <returns></returns>
        [AuthorizeAction(Action = AuthorizedAction.Scoring.Execution)]
        [HttpGet]
        [Route("{judge:int:range(1,5)}/{skill:int:range(1,10)}/{deduction:int:range(0,5)}")]
        public IHttpActionResult SetScore ([FromUri] int judge, [FromUri] int skill, [FromUri] int deduction)
        {
            ValidateWith<CurrentProviderWithSkillValidatorBuilder>(currentProvider, skill);

            currentProvider.CurrentScore.SetSkillDeduction(judge, skill, deduction);
            return Ok();
        }

        /// <summary>
        /// Sets excution judge's landing deduction.
        /// </summary>
        /// <param name="judge">The judge.</param>
        /// <param name="landing">The landing.</param>
        /// <returns></returns>
        [AuthorizeAction(Action = AuthorizedAction.Scoring.Execution)]
        [HttpGet]
        [Route("{judge:int:range(1,5)}/landing={landing:int:range(0,5)}")]
        public IHttpActionResult SetLanding ([FromUri] int judge, [FromUri] int landing)
        {
            ValidateWith<CurrentProviderWithScoringModelValidatorBuilder>(currentProvider);

            currentProvider.CurrentScore.SetLandingDeduction(judge, landing);
            return Ok();
        }

        /// <summary>
        /// Sets execution judge's additional deduction.
        /// </summary>
        /// <param name="judge">The judge.</param>
        /// <param name="penalty">The penalty.</param>
        /// <returns></returns>
        [AuthorizeAction(Action = AuthorizedAction.Scoring.Execution)]
        [HttpGet]
        [Route("penalty/{judge:int:range(0,4)}/{penalty:int:range(0,10)}")]
        public IHttpActionResult SetAdditionalDeduction ([FromUri] int judge, [FromUri] int penalty)
        {
            ValidateWith<CurrentProviderWithScoringModelValidatorBuilder>(currentProvider);

            currentProvider.CurrentScore.SetAdditionalDeduction(judge, penalty);
            return Ok();
        }

        /// <summary>
        /// Sets chair of judges panel's additional deduction.
        /// </summary>
        /// <param name="penalty">The penalty.</param>
        /// <returns></returns>
        [AuthorizeAction(Action = AuthorizedAction.Scoring.Penalty)]
        [HttpGet]
        [Route("penalty/{penalty:int:range(0,10)}")]
        public IHttpActionResult SetAdditionalDeduction ([FromUri] int penalty)
        {
            ValidateWith<CurrentProviderWithScoringModelValidatorBuilder>(currentProvider);

            currentProvider.CurrentScore.Penalty = penalty;
            return Ok();
        }

        /// <summary>
        /// Sets the difficulty.
        /// </summary>
        /// <param name="difficulty">The difficulty.</param>
        /// <returns></returns>
        [AuthorizeAction(Action = AuthorizedAction.Scoring.Difficulty)]
        [HttpGet]
        [Route("diff={difficulty:int:range(0,200)}")]
        public IHttpActionResult SetDifficulty (int difficulty)
        {
            ValidateWith<CurrentProviderWithScoringModelValidatorBuilder>(currentProvider);

            currentProvider.CurrentScore.Difficulty = difficulty;
            return Ok();
        }

        /// <summary>
        /// Sets the flight time.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        [AuthorizeAction(Action = AuthorizedAction.Scoring.Time)]
        [HttpGet]
        [Route("time={time:int:range(0,30000)}")]
        public IHttpActionResult SetFlightTime ([FromUri] int time)
        {
            ValidateWith<CurrentProviderWithScoringModelValidatorBuilder>(currentProvider);

            currentProvider.CurrentScore.Time = time;
            return Ok();
        }

        /// <summary>
        /// Preview unpublished score details.
        /// </summary>
        /// <returns></returns>
        [AuthorizeAction(Action = AuthorizedAction.Scoring.Preview)]
        [HttpGet]
        [Route("preview")]
        public Task<Routine> GetScoreDetails ()
        {
            ValidateWith<CurrentProviderWithScoringModelValidatorBuilder>(currentProvider);

            var proposed = currentProvider.CurrentScore.ComputeRoutineScore();
            return Task.FromResult(proposed);
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
            var routine = currentProvider.CurrentScore.ComputeRoutineScore();
            currentProvider.CurrentAthlete.AddRoutine(routine);

            // persist data
            Update(currentProvider.CurrentAthlete.Id, currentProvider.CurrentAthlete).Wait();

            // prepare for next
            currentProvider.CurrentScore = null;
            return Ok();
        }

        /// <summary>
        /// Helper method for validating current provider with specified validator.
        /// </summary>
        /// <typeparam name="TValidatorBuilder">The type of the validator builder.</typeparam>
        /// <param name="curr">The curr.</param>
        /// <param name="obj">The object.</param>
        private static void ValidateWith<TValidatorBuilder> (ICurrentProvider curr, object obj = null)
            where TValidatorBuilder : IValidatorBuilder<ICurrentProvider>, new()
        {
            var builder = new TValidatorBuilder();
            var validator = builder.Build();

            validator.Validate(curr, obj);
        }
    }
}
