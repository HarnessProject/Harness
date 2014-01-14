using System.Collections.Generic;
using System.Composition;
using System.Messaging;
using System.Portable.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Harness.Web.WebApi2 {
    public interface IHttpController : System.Web.Http.Controllers.IHttpController, IDependency {
        IScope LocalScope { get; set; }

    }

    public interface IHttpConfigProvider : IDependency {
        void Configure(HttpConfiguration config);
    }
}
