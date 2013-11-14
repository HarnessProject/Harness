using System;
using System.Threading.Tasks;
using Harness.Events;

namespace Harness {
    public interface IScope : IDisposable {
        IServiceLocator ServiceLocator { get; set; }
        IEnvironment Environment { get; set; }
        IDispatch Dispatcher { get; set; }
        IEventManager EventManager { get; set; }
    }
}