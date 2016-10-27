namespace SimplerScore.DataObjects
{
    using System.Collections.Generic;

    public class Routine
    {
        public List<Execution> Execution
        {
            get;
            set;
        }

        public int Difficulty
        {
            get;
            set;
        }

        public int FlightTime
        {
            get;
            set;
        }

        public decimal Score
        {
            get;
            set;
        }

        public string Video
        {
            get;
            set;
        }
    }
}
