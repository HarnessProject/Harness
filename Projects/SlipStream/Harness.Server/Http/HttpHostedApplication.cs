using System;
using System.Collections.Generic;
using System.Composition;
using System.Events;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Harness.Autofac;
using Harness.Http;
using Harness.TinyMessenger;
using Harness.Web.Owin;
using ImpromptuInterface;

namespace Harness.Server.Http
{
    public class HttpHostedApplication : IHostedApplication {
        protected Action<IScope> ScopeBuilder;
        public IScope NewScope {
            get {
                var scope = new Scope();
                ScopeBuilder(scope);
                return scope;
            }
        }
        public HttpHostedApplication(IHostedApplicationConfiguration config, params IApplication[] applications) {
            Config = config;
            Applications = applications;
            ScopeBuilder = x => {
                x.Container = new AutofacDependencyContainer(new HttpTypeProvider());
                x.MessengerHub = new TinyMessengerHub();
                x.EventMessenger = new EventMessenger(x.MessengerHub, x);
            };
            Scope = NewScope;
        }
        public IScope Scope { get; set; }
        public IHostedApplicationConfiguration Config { get; set; }
        public IApplication[] Applications { get; set; }
        public IHttpService Service { get; set; }
        public void StartService() {
            var listener = new HttpListener();
            listener.Prefixes.Add(string.Format("http://{0}:{1}", Config.HostName, Config.Port));
            listener.Start();
            Service = listener.ActLike();
        }

        public void StopService() {
            HttpListener listener = Service.UndoActLike().As<HttpListener>();
            listener.Stop();
        }
    }
}
