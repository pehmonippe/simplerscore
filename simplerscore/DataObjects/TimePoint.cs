namespace SimplerScore.DataObjects
{
    using System;
    using System.Collections.Generic;
    using LiteDB;

    public class TimePoint
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

        public TimeSpan Time
        {
            get;
            set;
        }

        public SchedulingBehavior Behavior
        {
            get;
            set;
        }

        public List<int> EventIds
        {
            get;
            set;
        } = new List<int>();

        public bool Interlaced
        {
            get;
            set;
        }
    }
}