using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace Harness.OWIN {
    public interface IApplication : IDependency {
        string BasePath { get; }
        void Configure(IAppBuilder app);
    }

    public interface IMiddleware : IDependency {
        Task Invoke(IOwinContext context, OwinMiddleware next);
    }
}