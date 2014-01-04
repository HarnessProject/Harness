using System;
using System.Composition;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Events;
using System.Runtime.Remoting.Channels;
using System.Tasks;

namespace Harness.Composition.CompositeEntities
{
    public interface IDataRepository : IDependency {
        ExternalDatabase Database { get; set; }
       
    }

    public class DatabaseValueChangedEvent : Event {}

    public class DataRepositiory : IDataRepository {
        private ExternalDatabase _database;
        private IDbDataAdapter _adoAdapter;
       
        private bool IsEF { get; set; }

        public ExternalDatabase Database {
            get { return _database; }
            set {
                _database = value;
                _adoAdapter = DbProviderFactories.GetFactory(Database.ProviderType).CreateDataAdapter();
                ApplicationScope.Global.EventMessenger.Trigger(new DatabaseValueChangedEvent(){ Sender = this});
            }
        }

       
    }

    public interface IEntity {}
}
