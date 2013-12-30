using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;

namespace Timesheets.Services.Controllers
{
    public class ManagementApiController : Harness.Web.WebApi2.IHttpController
    {
        public Task<HttpResponseMessage> ExecuteAsync(HttpControllerContext controllerContext, CancellationToken cancellationToken) {
            throw new NotImplementedException();
        }

        public IScope LocalScope { get; set; }
    }
}
