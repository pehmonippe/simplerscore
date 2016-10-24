namespace SimplerScore.Models
{
    using DataObjects;
    using JetBrains.Annotations;

    public class AthleteModel : Athlete
    {
        public AthleteModel ()
        {
        }

        public AthleteModel ([NotNull] Athlete athelete)
        {
            EventId = athelete.EventId;
            Excercises = athelete.Excercises;
            FirstName = athelete.FirstName;
            Id = athelete.Id;
            LastName = athelete.LastName;
            RunningOrder = athelete.RunningOrder;
            Team = athelete.Team;
            Total = athelete.Total;
        }
    }
}