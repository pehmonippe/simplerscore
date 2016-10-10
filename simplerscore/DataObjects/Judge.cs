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
    }
}
