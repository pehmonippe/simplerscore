namespace SimplerScore.DataObjects
{
    using System.Collections.Generic;
    using LiteDB;

    public class Athlete : Person
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

        public int RunningOrder
        {
            get;
            set;
        }

        public List<Exercise> Excercises
        {
            get;
            set;
        } = new List<Exercise>();

        public double Total
        {
            get;
            set;
        }
    }
}
