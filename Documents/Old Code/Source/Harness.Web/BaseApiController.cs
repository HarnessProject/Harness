using System.Composition;
using System.Web.Http;
using Autofac;

namespace Harness.Web {
    public abstract class BaseApiController : ApiController, IApiController {
        
        protected override void Dispose(bool disposing)
        {
            if (disposing) LocalScope.Dispose();
            base.Dispose(disposing);
        }

        public IScope LocalScope { get; set; }
    }
}