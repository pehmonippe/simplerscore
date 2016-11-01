namespace SimplerScore.DataObjects
{
    using System.Collections.Generic;

    public class Routine
    {
        public List<Execution> Executions
        {
            get;
            set;
        }

        public Execution Median
        {
            get;
            set;
        }

        public int Difficulty
        {
            get;
            set;
        }

        public int Time
        {
            get;
            set;
        }

        public int Penalty
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
