using System.Web.Http;

namespace Harness.Web.WebApi {
    public interface IApiController : IDependency { }
    public interface IConfig : IDependency
    {
        void Configure(HttpConfiguration config);
    }

}
