namespace SimplerScore.Models
{
    using System.Collections.Generic;
    using DataAccess;
    using DataObjects;

    public class MeetModel : Meet
    {
        private readonly IDataProvider<Meet> provider;

        public IEnumerable<EventModel> Events
        {
            get;
            set;
        } = new List<EventModel>();

        public MeetModel (IDataProvider<Meet> provider)
        {
            this.provider = provider;
        }

        public void LoadModel ()
        {
            if (0 == Id)
                return;

            var me = provider.Find(Id);
        }
    }

    public class EventModel : Event
    {
        private readonly IDataProvider<Event> provider;

        public IEnumerable<AthleteModel> Athletes
        {
            get;
            set;
        } = new List<AthleteModel>();

        public EventModel (IDataProvider<Event> provider)
        {
            this.provider = provider;
        }
    }

    public class AthleteModel : Athlete
    {
        private readonly IDataProvider<Athlete> provider;

        public AthleteModel (IDataProvider<Athlete> provider)
        {
            this.provider = provider;
        }
    }
}
