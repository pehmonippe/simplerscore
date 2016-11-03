namespace SimplerScore.Models
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class CurrentProvider : ICurrentProvider
    {
        #region Events

        #region PropertyChanged
        private event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged (string propertyName = null)
        {
            var handler = PropertyChanged;

            if (null == handler)
                return;

            handler(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        #endregion

        #region Properties

        #region CurrentMeet
        private MeetModel currentMeet;

        [CanBeNull]
        public MeetModel CurrentMeet
        {
            get { return currentMeet; }
            set { Set(ref currentMeet, value); }
        }
        #endregion

        #region CurrentEvent
        private EventModel currentEvent;

        [CanBeNull]
        public EventModel CurrentEvent
        {
            get { return currentEvent; }
            set { Set(ref currentEvent, value); }
        }
        #endregion

        #region CurrentAthlete
        private AthleteModel currentAthlete;

        [CanBeNull]
        public AthleteModel CurrentAthlete
        {
            get { return currentAthlete; }
            set { Set(ref currentAthlete, value); }
        }
        #endregion

        #region CurrentScore
        [CanBeNull]
        public ScoreModel CurrentScore
        {
            get;
            set;
        }
        #endregion

        #endregion

        public CurrentProvider ()
        {
            PropertyChanged += PropertyChanged_PropertyChanged;
        }

        private void PropertyChanged_PropertyChanged (object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(CurrentMeet):
                    CurrentEvent = null;
                    break;

                case nameof(CurrentEvent):
                    CurrentAthlete = null;
                    break;

                case nameof(CurrentAthlete):
                    CurrentScore = null;
                    break;

                default:
                    break;
            }
        }

        private void Set<T> (ref T property, T value, [CallerMemberName] string propertyName = null)
        {
            if (ReferenceEquals(property, value))
                return;

            property = value;
            RaisePropertyChanged(propertyName);
        }
    }
}