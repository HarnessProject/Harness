using System.Composition;
using System.Messaging;

namespace System {
    public interface IScope : IDisposable {
        IDependencyContainer Container { get; set; }
        IMessengerHub MessengerHub { get; set; }
      
    }
}