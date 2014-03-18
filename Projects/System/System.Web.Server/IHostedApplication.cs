using System.Composition;
using System.Web.Owin.Server.Http;

namespace System.Web.Server {
    public interface IHostedApplication {
        IScope Scope { get; }
        IHostedApplicationConfiguration Config { get; }
        IHttpService Service { get; }
        void Start();
        void Stop();
    }
}