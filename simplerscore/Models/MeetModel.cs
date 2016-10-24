namespace SimplerScore.Models
{
    using System;
    using DataObjects;
    using JetBrains.Annotations;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using DataAccess;

    public class MeetModel : Meet
    {
        private readonly IDataProvider provider;

        private Lazy<IEnumerable<EventModel>> events;

        public IEnumerable<EventModel> Events
        {
            get
            {
                var e = events ?? (events = new Lazy<IEnumerable<EventModel>>(InitModelCollection));
                return e.Value;
            }
        }

        public MeetModel ()
        {
        }

        public MeetModel ([NotNull] Meet meet)
            : this(meet, null)
        {
        }

        public MeetModel ([NotNull] Meet meet, [CanBeNull] IDataProvider provider)
        {
            DateOfEvent = meet.DateOfEvent;
            Id = meet.Id;
            Location = meet.Location;
            Name = meet.Name;
            Sponsor = meet.Sponsor;

            this.provider = provider;
        }

        private List<EventModel> InitModelCollection ()
        {
            if (null == provider)
                return new List<EventModel>();

            Expression<Func<Event, bool>> eventCriteria = e => e.MeetId == Id;

            var collection = provider.Collection<Event>()
                .Find(eventCriteria)
                .ToList()
                .ConvertAll(e => new EventModel(e, provider));

            return collection;
        }
    }

}
