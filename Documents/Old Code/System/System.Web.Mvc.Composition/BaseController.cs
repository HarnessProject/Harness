using System.Composition;

namespace System.Web.Mvc.Composition {
    public abstract class BaseController : Controller, IController {
        public IScope LocalScope { get; set; }
    }
}