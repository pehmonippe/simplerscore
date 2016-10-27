namespace SimplerScore.Models
{
    public interface ICurrentProvider
    {
        AthleteModel CurrentAthlete
        {
            get;
            set;
        }
        EventModel CurrentEvent
        {
            get;
            set;
        }
        MeetModel CurrentMeet
        {
            get;
            set;
        }
        ScoreModel CurrentScore
        {
            get;
            set;
        }
    }
}