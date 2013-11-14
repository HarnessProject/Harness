using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;

using Newtonsoft.Json.Linq;


namespace Harness.Site.Server.Controllers
{
    [RoutePrefix("server")]
    [Route("admin/{action}")]
    public class AdminController : ApiController
    {
        

        public async Task<JsonResult<JObject>> GetSetting(string key) {
            
        }
    }
}
