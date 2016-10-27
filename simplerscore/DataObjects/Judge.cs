namespace SimplerScore.DataObjects
{
    using LiteDB;

    public class Judge : Person
    {
        [BsonId(true)]
        public int Id
        {
            get;
            set;
        }

        public Judge Clone ()
        {
            return (Judge) MemberwiseClone();
        }
    }
}
