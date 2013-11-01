using System.Threading.Tasks;
using Microsoft.Owin;

namespace Harness.OWIN {
    public abstract class BaseMiddleWare : OwinMiddleware, IMiddleware {
        protected BaseMiddleWare(OwinMiddleware next) : base(next) {}

        public new OwinMiddleware Next { get; set; }

        #region IMiddleware Members

        public abstract Task Invoke(IOwinContext context, OwinMiddleware next);

        #endregion

        public override Task Invoke(IOwinContext context) {
            return Invoke(context, Next);

        }
    }
}