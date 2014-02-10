using System.Collections.Generic;
using System.Linq;

namespace System.Portable.Data
{
    public interface IDataProvider {
        IRepository<T> GetRepository<T>() where T : new();
        IQueryable<T> Query<T>() where T : new();
    }

    public interface IRepository<T> : IDisposable {
        IEnumerable<T> Select(Filter<T> predicate); 
        void Add(params T[] items);
        void Update(params T[] items);
        void Delete(params T[] items);
        void Commit();
        void Rollback();
    }

}
