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

    public class EventModel : Event
    {
        private readonly IDataProvider provider;

        private Lazy<IEnumerable<AthleteModel>> athletes;
        private Lazy<JudgePanelModel> judgePanel;

        public IEnumerable<AthleteModel> Athletes
        {
            get
            {
                var a = (athletes ?? (athletes = new Lazy<IEnumerable<AthleteModel>>(InitModelCollection)))
                    .Value;

                return a;
            }
        }


        public JudgePanelModel JudgePanel
        {
            get
            {
                var j = (judgePanel ?? (JudgePanel = new Lazy<JudgePanelModel>(InitJudgePanel)))
                    .Value;

                return j;
            }
        }

        public EventModel ()
        {
        }

        public EventModel ([NotNull] Event evnt, [CanBeNull] IDataProvider provider)
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

            this.provider = provider;
        }

        private List<AthleteModel> InitModelCollection ()
        {
            if (null == provider)
                return new List<AthleteModel>();

            Expression<Func<Athlete, bool>> athleteCriteria = e => e.EventId == Id;

            var collection = provider.Collection<Athlete>()
                .Find(athleteCriteria)
                .ToList()
                .ConvertAll(athlete => new AthleteModel(athlete));

            return collection;
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

    public class JudgePanelModel : JudgePanel
    {
        private readonly IDataProvider provider;

        public JudgePanelModel ()
        {
        }

        public JudgePanelModel ([NotNull] JudgePanel judgePanel, IDataProvider provider)
        {
            ChiefJudge = judgePanel.ChiefJudge;
            DifficultyJudge = judgePanel.DifficultyJudge;
            FlightTimeJudge = judgePanel.FlightTimeJudge;
            Id = judgePanel.Id;
            Judge1 = judgePanel.Judge1;
            Judge2 = judgePanel.Judge2;
            Judge3 = judgePanel.Judge3;
            Judge4 = judgePanel.Judge4;
            Judge5 = judgePanel.Judge5;

            this.provider = provider;
        }

        private JudgePanel InitJudgePanel ()
        {
            if (null == provider)
                return new JudgePanel();

            //TODO:...
            var panel = provider.Collection<JudgePanel>();
            return panel;
        }
    }
}
