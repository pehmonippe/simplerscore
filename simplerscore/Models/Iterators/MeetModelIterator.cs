namespace SimplerScore.Models.Iterators
{
    using System.Collections;
    using Extensions;
    using JetBrains.Annotations;
    using Utilities;

    public class MeetModelIterator : Disposable, IIterator<EventModel>
    {
        private readonly MeetModel rootModel;
        private EventModel eventModel;

        public MeetModelIterator ([NotNull] MeetModel rootModel)
        {
            this.rootModel = rootModel;
        }

        public bool MovePrevious ()
        {
            eventModel = eventModel.GetPreviousItem(rootModel.Events.ToScheduledOrder());
            return null != eventModel;
        }

        public bool MoveNext ()
        {
            eventModel = eventModel.GetFollowingItem(rootModel.Events.ToScheduledOrder());
            return null != eventModel;
        }

        public void Reset ()
        {
            ThrowIfDisposed();
            eventModel = null;
        }

        public EventModel Current
        {
            get
            {
                ThrowIfDisposed();
                return eventModel;
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
    }
}