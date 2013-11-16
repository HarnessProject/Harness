using System.Net.NetworkInformation;
using System.Web.Mvc;
using Autofac;

namespace Harness.Web {
    public abstract class BaseController : Controller, IController {
        public IScope LocalScope { get; set; }
    }
}