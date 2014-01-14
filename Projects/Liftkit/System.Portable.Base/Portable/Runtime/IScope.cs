using System.Collections.Generic;
using System.Composition;
using System.Messaging;
using System.Portable.Events;

namespace System.Portable.Runtime {
    public interface IScope : IDisposable {
        IDependencyProvider Container { get; set; }
        IMessengerHub MessengerHub { get; set; }
        IEventManager EventMessenger { get; set; }
        IDictionary<string, object> State { get; }

    }
}