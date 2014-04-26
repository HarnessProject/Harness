using System.Composition;
using System.Composition.Owin;
using System.Net;

namespace System.Web.Owin.Server.Http
{
    public class HttpHostedApplication : IHostedApplication {
        
        
        public HttpHostedApplication(IHostedApplicationConfiguration config, params IApplication[] applications) {
            Config = config;
            Applications = applications;
        }

        public IScope Scope { get; set; }
        public IHostedApplicationConfiguration Config { get; set; }
        public IApplication[] Applications { get; set; }
        public IHttpService Service { get; set; }
        public void Start() {
            var listener = new HttpListener();
            listener.Prefixes.Add(string.Format("http://{0}:{1}", Config.HostName, Config.Port));
            listener.Start();
            Service = listener.ActLike<IHttpService>();
        }

        public void Stop() {
            HttpListener listener = Service.UndoActLike().As<HttpListener>();
            listener.Stop();
        }
    }
}
