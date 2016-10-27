namespace SimplerScore.Models
{
    using DataAccess;
    using DataObjects;
    using JetBrains.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Factories;

    public interface IModel
    {
    }

    public class MeetModel : Meet, IModel
    {
        private readonly IDataProvider provider;
        private readonly IModelFactoryContainer modelFactoryContainer;

        private Lazy<IEnumerable<EventModel>> events;

        public IEnumerable<EventModel> Events
        {
            get
            {
                var e = events ?? (events = new Lazy<IEnumerable<EventModel>>(InitModelCollection));
                return e.Value;
            }
        }

        public MeetModel ([NotNull] IDataProvider provider, [NotNull] IModelFactoryContainer modelFactoryContainer)
        {
            this.provider = provider;
            this.modelFactoryContainer = modelFactoryContainer;
        }

        public MeetModel ([NotNull] Meet meet, [NotNull] IDataProvider provider, [NotNull] IModelFactoryContainer modelFactoryContainer)
            : this(provider, modelFactoryContainer)
        {
            DateOfEvent = meet.DateOfEvent;
            Id = meet.Id;
            Location = meet.Location;
            Name = meet.Name;
            Sponsor = meet.Sponsor;
        }

        private List<EventModel> InitModelCollection ()
        {
            if (null == provider)
                return new List<EventModel>();

            Expression<Func<Event, bool>> eventCriteria = e => e.MeetId == Id;

            var factory = modelFactoryContainer.ModelFactoryOfType<Event>();

            var collection = provider.Collection<Event>()
                .Find(eventCriteria)
                .ToList()
                .ConvertAll(e => (EventModel)factory.Create(e, provider, modelFactoryContainer));

            return collection;
        }
    }

}
