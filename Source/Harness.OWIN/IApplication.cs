using System;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Owin;
using Owin;

namespace Harness.Owin{
    public interface IApplication :IDependency  {
        string BasePath { get; }
        void Configure(IAppBuilder app);
    }

    public interface IMiddleware : IDependency,IDisposable {
        Task Invoke(IOwinContext context);
      
    }

    
}