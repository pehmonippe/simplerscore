namespace SimplerScore.Controllers
{
    using Attributes;
    using DataAccess;
    using DataObjects;
    using JetBrains.Annotations;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Validators;

    [RoutePrefix("score")]
    public class ScoringController : BaseController
    {
        [NotNull]
        private readonly ICurrentProvider currentProvider;

        public ScoringController ([NotNull] ICurrentProvider currentProvider, [NotNull] IDataProvider current)
            : base(current)
        {
            this.currentProvider = currentProvider;
        }

        [AuthorizeAction(Action = AuthorizedAction.Scoring.Preview)]
        [HttpGet]
        [Route("details")]
        public Task<ProposedScoring> GetScoreDetails ()
        {
            var model = currentProvider.CurrentScore;

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
            currentProvider.CurrentScore.SetSkillDeduction(judge, skill, deduction);
            return Ok();
        }

        [AuthorizeAction(Action = AuthorizedAction.Scoring.Execution)]
        [HttpGet]
        [Route("{judge:int:range(1,5)}/landing={landing:int:range(0,5)}")]
        public IHttpActionResult SetLanding ([FromUri] int judge, [FromUri] int landing)
        {
            currentProvider.CurrentScore.SetLandingDeduction(judge, landing);
            return Ok();
        }

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
        [Route("penalty/{judge:int:range(0,4)}/{penalty:int:range(0,10)}")]
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
            var routine = currentProvider.CurrentScore.ComputeRoutineScore();
            currentProvider.CurrentAthlete.AddRoutine(routine);

            // persist data
            Update(currentProvider.CurrentAthlete.Id, currentProvider.CurrentAthlete).Wait();

            // prepare for next
            currentProvider.CurrentScore = null;
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
