using System.Web.Http;

namespace Harness.Site.Server
{
    public interface IServerComponent : IDependency { }
    public interface ISingletonComponent : IServerComponent { }
    public interface ITransientComponent : IServerComponent { }

    public interface IApiRouteProvider : IServerComponent
    {
        void ProvideRoutes(HttpRouteCollection routes);
    }

    public interface IServerApplication : IServerComponent, Owin.IApplication { }

    public interface IService : IServerComponent
    {
        void Start(IEndPoint[] endpoints);
    }


    public interface IEndPoint
    {
        string Protocol { get; set; }
        string Port { get; set; }
        string Address { get; set; }
        IServerComponent Component { get; set; }
    }
}
