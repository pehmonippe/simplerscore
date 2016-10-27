namespace SimplerScore.Models
{
    using JetBrains.Annotations;

    public class CurrentProvider : ICurrentProvider
    {
        [CanBeNull]
        public MeetModel CurrentMeet
        {
            get;
            set;
        }

        [CanBeNull]
        public EventModel CurrentEvent
        {
            get;
            set;
        }

        [CanBeNull]
        public AthleteModel CurrentAthlete
        {
            get;
            set;
        }

        [CanBeNull]
        public ScoreModel CurrentScore
        {
            get;
            set;
        }
    }
}