using System.Collections.Generic;
using System.Composition;
using System.Net;
using System.Portable;
using System.Web.Owin.Server.Http;
using System.Web.Server;
using Microsoft.Owin.Builder;
using Nowin;
using Owin;

namespace System.Owin
{
    public class OwinHostedApplication : HostedApplication<OwinHandlerContext> {
       
        public IApplication[] Applications { get; set; }
       
        protected static T AsIf<T>(object o) where T: class{
            return Provider.Reflector.Impersonate<T>(o);
        }

        public OwinHostedApplication(IHostedApplicationConfiguration config, params IApplication[] applications) : base(config) {
            
            Applications = applications;

            Scope = Provider.Get<IScope>();
            
        }

        public override void Start() {
            var owinbuilder = new AppBuilder();
            OwinServerFactory.Initialize(owinbuilder.Properties);
            this.Configuration(owinbuilder);
            var builder = ServerBuilder.New();

            var ip = Config.IPAddress.Parse<IPAddress>() ?? IPAddress.None;

            Config.If(c => !string.IsNullOrEmpty(c.HostName), c => builder.SetServerHeader(c.HostName));
            Config.If(c => c.Port <= 0, c => builder.SetPort(c.Port), c => builder.SetPort(8888));
            Config.If(c => !Equals(ip, IPAddress.None), c => builder.SetAddress(ip), c => builder.SetAddress(IPAddress.Any));

            builder
                .SetOwinApp(owinbuilder.Build())
                .SetOwinCapabilities(
                    (IDictionary<string, object>)owinbuilder.Properties[OwinKeys.ServerCapabilitiesKey]
                );
            var service = builder.Build();
            Service = AsIf<IHttpService>(service); 
            Service.Start();
        }

        public override void Stop() {
            Service.Try(s => { 
                s.Stop();
                return true;
            });

            Service.Dispose();
            Service = null;
        }

        public async void Configuration(IAppBuilder app) {
            app.AsObservable(this);
        }
    }
}
