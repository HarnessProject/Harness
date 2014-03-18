using System.Composition;

namespace System.Owin{
    public abstract class BaseMiddleWare : IMiddleware {
        protected BaseMiddleWare() { }
        public IScope Scope { get; set; }

        public abstract void OnNext(OwinHandlerContext value);
        public abstract void OnError(Exception error);
        public abstract void OnCompleted();
    }
}