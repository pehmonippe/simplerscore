namespace SimplerScore.DataObjects
{
    using System;
    using System.Collections.Generic;
    using LiteDB;

    public class Schedule
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

        /// <summary>
        /// Gets or sets the interlaced events.
        /// Interlaced events are events in same group
        /// running in order of Order in subsequent
        /// immediately one after another in same round.
        /// </summary>
        /// <value>
        /// The interlaced events. Group id is used as key.
        /// </value>
        public Dictionary<int, List<int>> InterlacedEvents
        {
            get;
            set;
        }
    }

    //public class ScheduleEntry
    //{
    //    public int Id
    //    {
    //        get;
    //        set;
    //    }

    //    public int ScheduleId
    //    {
    //        get;
    //        set;
    //    }


    //    public int EventId
    //    {
    //        get;
    //        set;
    //    }


    //    public DateTime Time
    //    {
    //        get;
    //        set;
    //    }

    //    public int Group
    //    {
    //        get;
    //        set;
    //    }

    //    public int Order
    //    {
    //        get;
    //        set;
    //    }

    //    public SchedulingBehavior ScheduleBehavior
    //    {
    //        get;
    //        set;
    //    }
    //}
}
