namespace SimplerScore.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using DataAccess;
    using DataObjects;
    using Factories;
    using JetBrains.Annotations;

    public class TimePointModel : TimePoint, IModel
    {
        private readonly CollectionInitializer collectionInitializer;

        private Lazy<IEnumerable<EventModel>> events;

        public IEnumerable<EventModel> Events
        {
            get
            {
                var e = events ?? (events = new Lazy<IEnumerable<EventModel>>(InitEventCollection));
                return e.Value;
            }
        }

        public TimePointModel ([NotNull] IDataProvider provider, IModelFactoryContainer modelFactoryContainer)
        {
            collectionInitializer = new CollectionInitializer(provider, modelFactoryContainer);
        }

        public TimePointModel ([NotNull] TimePoint timePoint, IDataProvider provider, IModelFactoryContainer modelFactoryContainer)
            : this(provider, modelFactoryContainer)
        {
            Id = timePoint.Id;
            MeetId = timePoint.MeetId;
            Time = timePoint.Time;
            Behavior = timePoint.Behavior;
            EventIds = timePoint.EventIds;
            Interlaced = timePoint.Interlaced;
        }

        private List<EventModel> InitEventCollection ()
        {
            var collection = collectionInitializer.Initialize<Event, EventModel>(e => e.MeetId == MeetId && EventIds.Contains(e.Id));

            var sorted = collection
                .Select(e => new { Event = e, Rank = EventIds.IndexOf(e.Id) })
                .OrderBy(a => a.Rank)
                .Select(a => a.Event)
                .ToList();

            return sorted;
        }
    }
}