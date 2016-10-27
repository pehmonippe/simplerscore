namespace SimplerScore.Models
{
    using System.Collections.Generic;
    using DataObjects;
    using JetBrains.Annotations;

    public class AthleteModel : Athlete, IModel
    {
        public AthleteModel ()
        {
        }

        public AthleteModel ([NotNull] Athlete athlete)
        {
            EventId = athlete.EventId;
            Routines = athlete.Routines;
            FirstName = athlete.FirstName;
            Id = athlete.Id;
            LastName = athlete.LastName;
            RunningOrder = athlete.RunningOrder;
            Team = athlete.Team;
            Total = athlete.Total;
        }

        public void AddRoutine (Routine routine)
        {
            (Routines ?? new List<Routine>()).Add(routine);
            Total += routine.Score;
        }
    }
}