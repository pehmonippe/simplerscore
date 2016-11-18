namespace SimplerScore.DataObjects
{
    using System;
    using System.Collections.Generic;

    public class ScheduleEntry
    {
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

        public List<string> Events
        {
            get;
            set;
        } = new List<string>();
    }
}
