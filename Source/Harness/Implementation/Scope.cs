using System;
using System.Threading.Tasks;
using Harness.Events;

namespace Harness {
    public class Scope : IScope {
        public IServiceLocator ServiceLocator { get; set; }
        public IEnvironment Environment { get; set; }
        public IDispatch Dispatcher { get; set; }
        public IEventManager EventManager { get; set; }
        public Scope() {
            Environment = X.Environment;
            ServiceLocator = X.ServiceLocator;
            Dispatcher = ServiceLocator.GetInstance<IDispatch>();
            EventManager = ServiceLocator.GetInstance<IEventManager>();
        }

        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool dispose)
        {
            if (dispose) ServiceLocator.Dispose();
        }
    }
}