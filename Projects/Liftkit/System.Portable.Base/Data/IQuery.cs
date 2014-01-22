using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Data
{
    public interface IDataProvider {
        ISession<T> Session<T>() where T : new();
        IQueryable<T> Query<T>() where T : new();

    }

    public interface ISession<T> : IDisposable
    {
        IQueryable<T> Query();

        void Add(params T[] items);

        void Delete(params T[] items);

        void DeleteAll();

        void Commit();

        void Rollback();
    }

}
