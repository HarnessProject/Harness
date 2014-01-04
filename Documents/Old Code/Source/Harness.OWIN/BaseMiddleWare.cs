using System;
using System.Composition;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Owin;
using Owin;

namespace Harness.Owin{
    public abstract class BaseMiddleWare : OwinMiddleware, IMiddleware {
        protected BaseMiddleWare() : base(null) {}
        public IScope Scope { get; set; }

        public abstract override Task Invoke(IOwinContext context);

        public void Dispose() {
            Scope.Dispose();
        }
        
    }
}