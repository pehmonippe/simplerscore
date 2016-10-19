namespace SimplerScore.Models
{
    using DataObjects;
    using JetBrains.Annotations;
    using System.Collections.Generic;

    public class MeetModel : Meet
    {
        public IEnumerable<EventModel> Events
        {
            get;
            set;
        } = new List<EventModel>();

        public MeetModel ()
        {
        }

        public MeetModel ([NotNull] Meet meet)
        {
            DateOfEvent = meet.DateOfEvent;
            Id = meet.Id;
            Location = meet.Location;
            Name = meet.Name;
            Sponsor = meet.Sponsor;
        }
    }

    public class EventModel : Event
    {
        public IEnumerable<AthleteModel> Athletes
        {
            get;
            set;
        } = new List<AthleteModel>();

        public EventModel ()
        {
        }

        public EventModel ([NotNull] Event evnt)
        {
            Group = evnt.Group;
            Id = evnt.Id;
            MeetId = evnt.MeetId;
            Name = evnt.Name;
            Order = evnt.Order;
            Panel = evnt.Panel;
            ScheduleBehavior = evnt.ScheduleBehavior;
            ScheduledTime = evnt.ScheduledTime;
            Sponsor = evnt.Sponsor;
        }
    }

    public class AthleteModel : Athlete
    {
        public AthleteModel ()
        {
        }

        public AthleteModel ([NotNull] Athlete athelete)
        {
            EventId = athelete.EventId;
            Excercises = athelete.Excercises;
            FirstName = athelete.FirstName;
            Id = athelete.Id;
            LastName = athelete.LastName;
            RunningOrder = athelete.RunningOrder;
            Team = athelete.Team;
            Total = athelete.Total;
        }
    }
}
