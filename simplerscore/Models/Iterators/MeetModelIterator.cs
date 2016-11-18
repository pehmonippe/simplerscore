namespace SimplerScore.Models.Iterators
{
    using System;
    using JetBrains.Annotations;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Utilities;

    public class MeetModelIterator : Disposable, IIterator<EventModel>//, IIterable<EventModel2>
    {
        private readonly MeetModel rootModel;
        private EventModel eventModel;
        private Lazy<List<int>> iterables;
        private int iterationIndex = -1;

        private List<int> Iterables
        {
            get
            {
                var i = iterables ?? (iterables = new Lazy<List<int>>(InitIterables));
                return i.Value;
            }
        }

        public MeetModelIterator ([NotNull] MeetModel rootModel)
        {
            this.rootModel = rootModel;
        }

        public bool MovePrevious ()
        {
            if (iterationIndex > 0)
            {
                iterationIndex--;
                eventModel = rootModel[Iterables[iterationIndex]];
            }
            else
            {
                eventModel = null;
            }

            return null != eventModel;
        }

        public bool MoveNext ()
        {
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (++iterationIndex < Iterables.Count)
            {
                eventModel = rootModel[Iterables[iterationIndex]];
            }
            else
            {
                eventModel = null;
            }

            return null != eventModel;
        }

        public void Reset ()
        {
            ThrowIfDisposed();

            iterationIndex = -1;
            eventModel = null;
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        public EventModel Current
        {
            get
            {
                ThrowIfDisposed();
                return eventModel;
            }
        }

        //public IIterator<EventModel> GetIterator ()
        //{
        //    return this;
        //}

        //IEnumerator<EventModel> IEnumerable<EventModel>.GetEnumerator ()
        //{
        //    return GetIterator();
        //}

        public IEnumerator GetEnumerator ()
        {
            return this;
        }

        private List<int> InitIterables ()
        {
            var ids = rootModel.TimePoints
                .OrderBy(tp => tp.Time)
                .SelectMany(tp => tp.EventIds)
                .ToList();

            return ids;
        }
    }
}
