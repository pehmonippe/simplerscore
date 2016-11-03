namespace SimplerScore.Models.Iterators
{
    using System.Collections;
    using Extensions;
    using JetBrains.Annotations;
    using Utilities;

    public class EventModelIterator : Disposable, IIterator<AthleteModel>
    {
        private readonly EventModel rootModel;
        private AthleteModel athleteModel;

        public EventModelIterator ([NotNull] EventModel rootModel)
        {
            this.rootModel = rootModel;
        }

        public bool MovePrevious ()
        {
            ThrowIfDisposed();

            athleteModel = athleteModel.GetPreviousItem(rootModel.Athletes.ToRunningOrder());
            return null != athleteModel;
        }

        public bool MoveNext ()
        {
            ThrowIfDisposed();

            athleteModel = athleteModel.GetFollowingItem(rootModel.Athletes.ToRunningOrder());
            return null != athleteModel;
        }

        public void Reset ()
        {
            ThrowIfDisposed();
            athleteModel = null;
        }

        public AthleteModel Current
        {
            get
            {
                ThrowIfDisposed();
                return athleteModel;
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}