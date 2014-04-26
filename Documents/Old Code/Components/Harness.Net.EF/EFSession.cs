using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace System.Data {
    public class EFSession<T> : ISession<T> where T : class {
        protected DbContext Context;
        protected DbSet<T> Set;

        public EFSession(DbContext context) {
            Set = context.Set<T>();
        } 

        public void Dispose() {
            
        }

        public IQueryable<T> Query() {
            return Set;
        }

        public void Add(params T[] items) {
            Set.AddRange(items);
        }

        public void Delete(params T[] items) {
            Set.RemoveRange(items);
        }

        public void DeleteAll() {
            throw new NotImplementedException();
        }

        public void Commit() {
            Context.SaveChanges();
        }

        public void Rollback() {
            throw new NotImplementedException();
        }
    }
}