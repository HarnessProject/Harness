using System;
using System.Threading.Tasks;
using Autofac;
using Microsoft.Owin;
using Owin;

namespace Harness.Owin{
    public abstract class BaseMiddleWare : OwinMiddleware, IMiddleware {
        protected BaseMiddleWare() : base(null) {}
        protected ILifetimeScope Scope = Application.CurrentEnvironment.Container.BeginLifetimeScope();

        protected T Resolve<T>() {
            return Scope.Resolve<T>();
        }

        public abstract override Task Invoke(IOwinContext context);

        public void Dispose() {
            Scope.Dispose();
        }
        
    }
}