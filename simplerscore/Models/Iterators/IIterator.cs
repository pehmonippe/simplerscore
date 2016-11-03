namespace SimplerScore.Models.Iterators
{
    using System.Collections.Generic;

    /// <summary>
    /// IIterator is bidirectional implementation of an ienumerator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIterator<out T> : IEnumerator<T>
    {
        bool MovePrevious ();
    }
}