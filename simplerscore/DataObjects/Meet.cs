namespace SimplerScore.DataObjects
{
    using System;
    using LiteDB;

    public class Meet
    {
        [BsonId(true)]
        public int Id
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public DateTime DateOfEvent
        {
            get;
            set;
        }

        public string Location
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
