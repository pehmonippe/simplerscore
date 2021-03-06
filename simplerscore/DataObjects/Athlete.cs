﻿namespace SimplerScore.DataObjects
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

        public List<Routine> Routines
        {
            get;
            set;
        } = new List<Routine>();

        public decimal Total
        {
            get;
            set;
        }
    }
}
