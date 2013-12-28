using System.Composition;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace Harness.Web.Owin{
    public abstract class BaseMiddleWare : OwinMiddleware, IMiddleware {
        protected BaseMiddleWare() : base(null) {}
        public IScope Scope { get; set; }

        public abstract override Task Invoke(IOwinContext context);

        public void Dispose() {
            Scope.Dispose();
        }
        
    }
}