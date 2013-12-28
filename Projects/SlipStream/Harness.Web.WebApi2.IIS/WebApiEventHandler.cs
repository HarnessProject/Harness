using System;
using System.Composition;
using System.Events;
using System.Web.Http;

namespace Harness.Web.WebApi2.AspNet
{
    public class WebApiEventHandler : IHandle<WebApiReadyEvent>
    {
        public void Handle(WebApiReadyEvent tEvent) {
            Application.Global.State["HttpConfig"].As<Action<HttpConfiguration>>()(GlobalConfiguration.Configuration);
        }
    }
}
