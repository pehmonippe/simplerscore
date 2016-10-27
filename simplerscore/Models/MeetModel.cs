namespace SimplerScore.Models
{
    using DataAccess;
    using DataObjects;
    using Extensions;
    using JetBrains.Annotations;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class MeetModel : Meet
    {
        private readonly IServiceProvider serviceProvider;

        private Lazy<IEnumerable<EventModel>> events;

        public IEnumerable<EventModel> Events
        {
            get
            {
                var e = events ?? (events = new Lazy<IEnumerable<EventModel>>(InitModelCollection));
                return e.Value;
            }
        }

        public MeetModel ([NotNull] Meet meet, [NotNull] IServiceProvider serviceProvider)
        {
            DateOfEvent = meet.DateOfEvent;
            Id = meet.Id;
            Location = meet.Location;
            Name = meet.Name;
            Sponsor = meet.Sponsor;

            this.serviceProvider = serviceProvider;
        }

        private List<EventModel> InitModelCollection ()
        {
            var provider = serviceProvider.GetService<IDataProvider>();

            if (null == provider)
                return new List<EventModel>();

            Expression<Func<Event, bool>> eventCriteria = e => e.MeetId == Id;

            var collection = provider.Collection<Event>()
                .Find(eventCriteria)
                .ToList()
                .ConvertAll(e => new EventModel(serviceProvider, e));

            return collection;
        }
    }

}
