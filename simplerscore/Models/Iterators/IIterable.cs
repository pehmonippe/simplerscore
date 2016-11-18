namespace SimplerScore.Models.Iterators
{
    using System.Collections.Generic;

    /// <summary>
    /// IIteratable returns bidirectional enumerator (IIterator).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIterable<out T> : IEnumerable<T>
    {
        IIterator<T> GetIterator ();
    }
}