using System;

using Harness.Messaging;


namespace Harness {
    public interface IScope : IDisposable {
        IServiceLocator ServiceLocator { get; set; }
        IMessengerHub MessengerHub { get; set; }
      
    }
}