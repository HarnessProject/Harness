using System.Portable.Runtime;
using System.Threading.Tasks;
using Microsoft.Owin;

namespace System.Composition.Owin{
    public abstract class BaseMiddleWare : IMiddleware {
        protected BaseMiddleWare() { }
        public IScope Scope { get; set; }

        public abstract Task Invoke(OwinHandlerContext context);

       
        
    }
}