namespace SimplerScore.DataObjects
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using JetBrains.Annotations;

    public class SkillEnumerator : IEnumerator<List<int>>, IEnumerable<List<int>>
    {
        private int index = -1;
        private readonly List<Execution> executions;
        private bool disposed;

        public SkillEnumerator ([NotNull, ItemNotNull] IEnumerable<Execution> executions)
        {
            this.executions = executions as List<Execution> ?? executions.ToList();
        }

        public SkillEnumerator ([NotNull] Routine routine)
            : this(routine.Executions)
        {
        }

        #region IEnumerator interface methods
        public void Dispose ()
        {
            disposed = true;
        }

        public bool MoveNext ()
        {
            ThrowIfDisposed();

            var hasMore = executions.Count < ++index;
            return hasMore;
        }

        public void Reset ()
        {
            ThrowIfDisposed();
            index = -1;
        }

        public List<int> Current
        {
            get
            {
                ThrowIfDisposed();

                var elementDeductions = executions
                    .Select(e => e.Elements[index])
                    .ToList();

                return elementDeductions;
            }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }
        #endregion

        #region IEnumerable interface methods
        public IEnumerator<List<int>> GetEnumerator ()
        {
            ThrowIfDisposed();
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator ()
        {
            ThrowIfDisposed();
            return GetEnumerator();
        }
        #endregion

        #region private methods
        private void ThrowIfDisposed ([CallerMemberName] string callingMethod = null)
        {
            if (!disposed)
                return;

            throw new ObjectDisposedException(callingMethod);
        }
        #endregion
    }
}