namespace SimplerScore.DataObjects
{
    using LiteDB;

    public class JudgePanel
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

        public Judge ChiefJudge
        {
            get;
            set;
        }

        public Judge Judge1
        {
            get;
            set;
        }

        public Judge Judge2
        {
            get;
            set;
        }

        public Judge Judge3
        {
            get;
            set;
        }

        public Judge Judge4
        {
            get;
            set;
        }

        public Judge Judge5
        {
            get;
            set;
        }

        public Judge DifficultyJudge
        {
            get;
            set;
        }

        public Judge FlightTimeJudge
        {
            get;
            set;
        }
    }
}
