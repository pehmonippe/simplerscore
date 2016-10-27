namespace SimplerScore.DataObjects
{
    using System.Collections.Generic;
    using LiteDB;

    public class JudgesPanel
    {
        [BsonId(true)]
        public int Id
        {
            get;
            set;
        }

        public int EventId
        {
            get;
            set;
        }

        public Judge ChairOfJudgesPanel
        {
            get;
            set;
        }

        public List<Judge> ExecutionJudges
        {
            get;
            set;
        }

        public Judge DifficultyJudge
        {
            get;
            set;
        }

        public Judge TimeJudge
        {
            get;
            set;
        }
    }
}
