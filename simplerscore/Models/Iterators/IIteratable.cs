namespace SimplerScore.Models.Iterators
{

    /// <summary>
    /// IIteratable returns bidirectional enumerator (IIterator).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IIteratable<out T>
    {
        IIterator<T> GetIterator ();
    }
}