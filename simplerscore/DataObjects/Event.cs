namespace SimplerScore.DataObjects
{
    using LiteDB;
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
