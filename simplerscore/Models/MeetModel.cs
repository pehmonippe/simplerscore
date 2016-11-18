namespace SimplerScore.Models
{
    using DataAccess;
    using DataObjects;
    using Factories;
    using JetBrains.Annotations;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Iterators;

    public class MeetModel : Meet, IModel, IIterable<EventModel>
    {
        private readonly IDataProvider provider;
        private readonly IModelFactoryContainer modelFactoryContainer;
        private readonly CollectionInitializer collectionInitializer;

        #region Events

        private Lazy<IEnumerable<EventModel>> events;

        public IEnumerable<EventModel> Events
        {
            get
            {
                var e = events ?? (events = new Lazy<IEnumerable<EventModel>>(InitModelCollection));
                return e.Value;
            }
        }

        #endregion

        #region TimePoints

        private Lazy<IEnumerable<TimePointModel>> timePoints;

        public IEnumerable<TimePointModel> TimePoints
        {
            get
            {
                var t = timePoints ?? (timePoints = new Lazy<IEnumerable<TimePointModel>>(InitTimePointCollection));
                return t.Value;
            }
        }
        #endregion


        public EventModel this[int eventId]
        {
            get
            {
                var evnt = Events.FirstOrDefault(e => e.Id == eventId);
                return evnt;
            }
        }

        public MeetModel ([NotNull] IDataProvider provider, [NotNull] IModelFactoryContainer modelFactoryContainer)
        {
            this.provider = provider;
            this.modelFactoryContainer = modelFactoryContainer;

            collectionInitializer = new CollectionInitializer(provider, modelFactoryContainer);
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
            var collection = collectionInitializer.Initialize<Event, EventModel>(e => e.MeetId == Id);
            return collection;
        }

        private List<TimePointModel> InitTimePointCollection ()
        {
            var collection = collectionInitializer.Initialize<TimePoint, TimePointModel>(t => t.MeetId == Id)
                .OrderBy(p => p.Time)
                .ToList();

            return collection;
        }

        public IIterator<EventModel> GetIterator ()
        {
            var iterator = new MeetModelIterator(this);
            return iterator;
        }

        public IEnumerator<EventModel> GetEnumerator ()
        {
            return Events.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            return GetEnumerator();
        }
    }

}
