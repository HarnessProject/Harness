using System.Portable.Runtime;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace System.Composition.Owin{
    public abstract class BaseMiddleWare : OwinMiddleware, IMiddleware {
        protected BaseMiddleWare() : base(null) {}
        public IScope Scope { get; set; }

        public abstract override Task Invoke(IOwinContext context);

        public void Dispose() {
            Scope.Dispose();
        }
        
    }
}