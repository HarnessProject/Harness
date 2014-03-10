using System.Composition;
using System.Portable.Runtime;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace Harness.Web.Owin{
    public interface IApplication : IDependency  {
        
        string BasePath { get; }
        void Configure(IAppBuilder app);
    }

    public interface IMiddleware : IDisposableDependency {
        Task Invoke(IOwinContext context);
    }

    
}