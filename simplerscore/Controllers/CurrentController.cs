namespace SimplerScore.Controllers
{
    using System.Linq;
    using DataAccess;
    using JetBrains.Annotations;
    using System.Web.Http;
    using Attributes;
    using DataObjects;
    using Exceptions;
    using Models;
    using Models.Factories;
    using Validators;

    [RoutePrefix("current")]
    public class CurrentController : BaseController
    {
        private readonly ICurrentProvider currentProvider;
        private readonly IModelFactoryContainer modelFactoryContainer;

        public CurrentController ([NotNull] ICurrentProvider currentProvider, [NotNull] IDataProvider provider, [NotNull] IModelFactoryContainer modelFactoryContainer) 
            : base(provider)
        {
            this.currentProvider = currentProvider;
            this.modelFactoryContainer = modelFactoryContainer;
        }

        [AuthorizeAction (Action = AuthorizedAction.Scoring.Navigate)]
        [HttpGet]
        [Route ("{meetId:int}")]
        public IHttpActionResult SetMeet ([FromUri] int meetId)
        {
            var meet = Provider.Find<Meet>(meetId);

            if (null == meet)
                throw new EntityNotFoundException();

            currentProvider.CurrentMeet = (MeetModel) modelFactoryContainer.Create(meet, Provider);
            currentProvider.CurrentEvent = currentProvider.CurrentMeet?.Events
                .OrderBy(e => e.Group)
                .ThenBy(e => e.Order)
                .FirstOrDefault();

            return Ok();
        }

        [AuthorizeAction(Action = AuthorizedAction.Scoring.Navigate)]
        [HttpGet]
        [Route("next")]
        public IHttpActionResult MoveToNext ()
        {
            ValidatorBuilderHelper.ValidateWith<CurrentProviderWithEventModelValidator>(currentProvider);

            //TODO: meetmodel enumerator to encapsule forward and backward enumeration.

            AthleteModel next;

            if (null == currentProvider.CurrentAthlete)
            {
                next = currentProvider.CurrentEvent.Athletes
                    .OrderBy(a => a.RunningOrder)
                    .FirstOrDefault();
            }
            else
            {
                next = currentProvider.CurrentEvent.Athletes
                    .Where(a => a.RunningOrder > currentProvider.CurrentAthlete.RunningOrder)
                    .OrderBy(a => a.RunningOrder)
                    .FirstOrDefault();
            }

            currentProvider.CurrentAthlete = next;
            return Ok();
        }

        [AuthorizeAction(Action = AuthorizedAction.Scoring.Navigate)]
        [HttpGet]
        [Route("previous")]
        public IHttpActionResult MoveToPrevious ()
        {
            ValidatorBuilderHelper.ValidateWith<CurrentProviderWithEventModelValidator>(currentProvider);

            AthleteModel previous;

            if (null == currentProvider.CurrentAthlete)
            {
                previous = currentProvider.CurrentEvent.Athletes
                    .OrderBy(a => a.RunningOrder)
                    .LastOrDefault();
            }
            else
            {
                previous = currentProvider.CurrentEvent.Athletes
                    .Where(a => a.RunningOrder > currentProvider.CurrentAthlete.RunningOrder)
                    .OrderByDescending(a => a.RunningOrder)
                    .FirstOrDefault();
            }

            currentProvider.CurrentAthlete = previous;
            return Ok();
        }

        [AuthorizeAction(Action = AuthorizedAction.Scoring.Navigate)]
        [HttpGet]
        [Route("current/{eventId:int}/{athleteId:int}")]
        public IHttpActionResult MoveTo ([FromUri] int eventId, [FromUri] int athleteId)
        {
            ValidatorBuilderHelper.ValidateWith<CurrentProviderWithEventModelValidator>(currentProvider);

            if (null == currentProvider.CurrentEvent || eventId != currentProvider.CurrentEvent.Id)
            {
                var currentEvent = currentProvider.CurrentMeet[eventId];
                currentProvider.CurrentEvent = currentEvent;
            }

            if (null != currentProvider.CurrentEvent)
            {
                var currentAthlete = currentProvider.CurrentEvent[athleteId];
                currentProvider.CurrentAthlete = currentAthlete;
            }
            else
            {
                currentProvider.CurrentAthlete = null;
            }

            if (null == currentProvider.CurrentAthlete)
                throw new NoCurrentException();

            return Ok();
        }
    }

}
