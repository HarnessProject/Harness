using System.Linq;

namespace System.Data {
    public class EFDatabase : IDataProvider {
        public ISession<T> Session<T>() where T : new() {
            throw new NotImplementedException();
        }

        public IQueryable<T> Query<T>() where T : new() {
            throw new NotImplementedException();
        }
    }
}