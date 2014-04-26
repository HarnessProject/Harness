using System.Composition;
using System.Portable.Runtime;
using Harness.Server.Http;
using Harness.Web.Owin;

namespace Harness.Server {
    public interface IHostedApplication {
        IScope Scope { get; set; }
        IHostedApplicationConfiguration Config { get; set; }
        IApplication[] Applications { get; set; }
        IHttpService Service { get; set; }
        void Start();
        void Stop();
    }
}