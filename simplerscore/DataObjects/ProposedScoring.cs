namespace SimplerScore.DataObjects
{
    using System.Collections.Generic;

    public class ProposedScoring
    {
        public List<Execution> Executions
        {
            get;
            set;
        }

        public int Time
        {
            get;
            set;
        }

        public int Difficulty
        {
            get;
            set;
        }

        public int Penalty
        {
            get;
            set;
        }
    }
}