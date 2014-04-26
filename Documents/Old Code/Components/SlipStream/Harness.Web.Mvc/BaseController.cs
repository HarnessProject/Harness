using System.Composition;
using System.Portable.Runtime;
using System.Web.Mvc;

namespace Harness.Web.Mvc {
    public abstract class BaseController : Controller, IController {
        public IScope LocalScope { get; set; }
    }
}