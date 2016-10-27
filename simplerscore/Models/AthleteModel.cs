namespace SimplerScore.Models
{
    using System.Collections.Generic;
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
            Routines = athelete.Routines;
            FirstName = athelete.FirstName;
            Id = athelete.Id;
            LastName = athelete.LastName;
            RunningOrder = athelete.RunningOrder;
            Team = athelete.Team;
            Total = athelete.Total;
        }

        public void AddRoutine (Routine routine)
        {
            (Routines ?? new List<Routine>()).Add(routine);
            Total += routine.Score;
        }
    }
}