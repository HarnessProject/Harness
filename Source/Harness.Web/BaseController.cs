using System.Net.NetworkInformation;
using System.Web.Mvc;
using Autofac;

namespace Harness.Web {
    public abstract class BaseController : Controller, IController {
        private readonly ILifetimeScope _scope;

        protected BaseController() : base() {
            _scope = X.Environment.Container.BeginLifetimeScope();
        }

        protected T Resolve<T>()
        {
            return _scope.Resolve<T>();
        }

        protected override void Dispose(bool disposing) {
            if (disposing) _scope.Dispose();
            base.Dispose(disposing);
        }
        
    }
}