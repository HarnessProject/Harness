using System.Web.Http;
using Autofac;

namespace Harness.Web.WebApi {
    public abstract class BaseApiController : ApiController, IApiController {
        protected readonly ILifetimeScope Scope = Application.CurrentEnvironment.Container.BeginLifetimeScope();
        
        protected T Resolve<T>() {
            return Scope.Resolve<T>();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing) Scope.Dispose();
            base.Dispose(disposing);
        }
    }
}