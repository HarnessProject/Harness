using System.Composition.Dependencies;
using System.Threading.Tasks;
using Owin;

namespace System.Owin{
    public interface IApplication : IDependency  {
        
        string BasePath { get; }
        void Configure(IAppBuilder app);
    }

    public interface IMiddleware : IObserver<OwinHandlerContext>, ITransientDependency {
        
    }

    
}