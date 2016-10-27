namespace SimplerScore.DataObjects
{
    using LiteDB;
    using System;
    using Models.Computation;

    public class Event
    {
        [BsonId(true)]
        public int Id
        {
            get;
            set;
        }

        public int MeetId
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public int Group
        {
            get;
            set;
        }

        public int Order
        {
            get;
            set;
        }

        public DateTime ScheduledTime
        {
            get;
            set;
        }

        public SchedulingBehavior ScheduleBehavior
        {
            get;
            set;
        }

        public JudgesPanel Panel
        {
            get;
            set;
        }

        public ComputationStrategy ScoringStrategy
        {
            get;
            set;
        }

        public string Sponsor
        {
            get;
            set;
        }
    }
}
