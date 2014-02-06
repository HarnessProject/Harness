using System.Collections.Generic;
using System.Dynamic;
using System.Messaging;
using System.Portable.Events;
using System.Portable.Messaging;

namespace System.Portable.Runtime {
    public class Scope : IScope {
        public Scope() {
            State = new ExpandoObject();
        }

        #region IScope Members

        public IDictionary<string, object> State { get; private set; }
        public IDependencyProvider Container { get; set; }
        public IMessengerHub MessengerHub { get; set; }
        public IEventManager EventManager { get; set; }

        public void Dispose() {
            Container.Dispose();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}