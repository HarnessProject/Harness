using System.Collections.Generic;
using System.Events;
using System.Messaging;

namespace System.Composition {
    public interface IScope : IDisposable {
        IDependencyContainer Container { get; set; }
        IMessengerHub MessengerHub { get; set; }
        IEventMessenger EventMessenger { get; set; }
        IDictionary<string, object> State { get; }

    }
}