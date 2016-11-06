namespace SimplerScore.Controllers
{
    using System;
    using System.Linq;
    using DataAccess;
    using JetBrains.Annotations;
    using System.Web.Http;
    using Attributes;
    using DataObjects;
    using Exceptions;
    using Models;
    using Models.Factories;
    using Models.Iterators;
    using Validators;

    [RoutePrefix ("current")]
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

        [AuthorizeAction (Action = AuthorizedAction.Scoring.Navigate)]
        [HttpGet]
        [Route ("next")]
        public IHttpActionResult MoveToNext ()
        {
            ValidatorBuilderHelper.ValidateWith<CurrentProviderWithEventModelValidator>(currentProvider);

            AthleteModel athleteModel;

            do
            {
                athleteModel = GetNext<AthleteModel>();

                // ReSharper disable once InvertIf
                if (null == athleteModel)
                {
                    var nextEvent = GetNext<EventModel>();
                    currentProvider.CurrentEvent = nextEvent;

                    if (null == currentProvider.CurrentEvent)
                        throw new NoCurrentException();
                }
            } while (null == athleteModel);

            currentProvider.CurrentAthlete = athleteModel;

            if (null == currentProvider)
                throw new NoCurrentException();

            return Ok();
        }

        [AuthorizeAction (Action = AuthorizedAction.Scoring.Navigate)]
        [HttpGet]
        [Route ("previous")]
        public IHttpActionResult MoveToPrevious ()
        {
            ValidatorBuilderHelper.ValidateWith<CurrentProviderWithEventModelValidator>(currentProvider);

            AthleteModel athleteModel;

            do
            {
                athleteModel = GetPrevious<AthleteModel>();

                // ReSharper disable once InvertIf
                if (null == athleteModel)
                {
                    var previousEvent = GetPrevious<EventModel>();
                    currentProvider.CurrentEvent = previousEvent;

                    if (null == currentProvider.CurrentEvent)
                        throw new NoCurrentException();
                }
            } while (null == athleteModel);

            currentProvider.CurrentAthlete = athleteModel;

            if (null == currentProvider)
                throw new NoCurrentException();

            return Ok();
        }

        [AuthorizeAction (Action = AuthorizedAction.Scoring.Navigate)]
        [HttpGet]
        [Route ("current/{eventId:int}/{athleteId:int}")]
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

        private IIterator<TModel> GetIterator<TModel> ()
        {
            var source = currentProvider as IIteratable<TModel>;

            if (null == source)
                throw new InvalidOperationException();

            var iterator = source.GetIterator();
            return iterator;
        }

        private TModel GetNext<TModel> ()
            where TModel : class
        {
            var next = Navigate<TModel>(iterator => iterator?.MoveNext() ?? false);
            return next;
        }

        private TModel GetPrevious<TModel> ()
            where TModel : class
        {
            var previous = Navigate<TModel>(iterator => iterator?.MovePrevious() ?? false);
            return previous;
        }

        private TModel Navigate<TModel> (Func<IIterator<TModel>, bool> navigateFunc)
            where TModel : class
        {
            var iterator = GetIterator<TModel>();

            var next = navigateFunc(iterator) ?
                iterator.Current :
                null;

            return next;
        }
    }
}
