using System.Composition.Dependencies;
using System.Web.Http;

namespace System.Composition.Owin.WebApi2 {
    public interface IHttpController : System.Web.Http.Controllers.IHttpController, IDependency {
        IScope LocalScope { get; set; }

    }

    public interface IHttpConfigProvider : IDependency {
        void Configure(HttpConfiguration config);
    }
}
