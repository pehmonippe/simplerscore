using System;
using LiteDB;

namespace SimplerScore.DataAccess
{
    public interface IDataProvider : IDisposable
    {
        IDisposable BeginScope ();

        LiteCollection<T> Collection<T> () 
            where T : class, new();

        void Commit (IDisposable disposable);
    }
}