using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Portable.Data;
using System.Portable.Reflection;
using System.Text;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.OrmLite;

namespace System.Data
{
    public class OrmLiteDataProvider : IDataProvider, IFactory<IDataProvider> {
        public string Name { get; set; }
        public string ConnectionString { get; set; }
        public OrmLiteConnectionFactory ConnectionFactory 
        { get { return new OrmLiteConnectionFactory(ConnectionString); } }
        
        public IRepository<T> Repository<T>() where T : class, new() {
            return new OrmLiteDataRepository<T>(ConnectionFactory.CreateDbConnection());
        }

        public IDataProvider Create(dynamic context) {
            if (context.Is<string>()) ConnectionString = context.As<string>();
            return this;
        }
    }

    public class OrmLiteDataRepository<T> : IRepository<T> where T : class, new() {
        private readonly IDbConnection _connection;
        private readonly IDbTransaction _transaction;

        public OrmLiteDataRepository(IDbConnection connection) {
            _connection = connection;
            _transaction = _connection.BeginTransaction();
        }

        public void Dispose() {
            _transaction.Commit();
            _connection.Dispose();
        }

        protected virtual IList<T> CreateResultSet() {
            //Override 
            return new List<T>();
        }

        public IEnumerable<T> GetByKey<TKey>(params TKey[] keys) {
            return _connection.SelectByIds<T>(keys);
        }

        public IList<T> Select(Expression<Filter<T>> filter) {
            return _connection.Select(Expression.Lambda<Func<T, bool>>(filter.Body));
        }

        public void Add(params T[] items) {
            _connection.Insert(items);
        }

        public int Update(params T[] items) {
            return _connection.Update(items);
        }

        public int Delete(params T[] items) {
            return _connection.Delete(items);
        }

        public void Abort() {
            _transaction.Rollback();
            _connection.Close();
        }
    }
}
