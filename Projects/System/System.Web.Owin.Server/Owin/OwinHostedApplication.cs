using System.Collections.Generic;
using System.Composition;
using System.Composition.Owin;
using System.Net;
using System.Portable;
using System.Threading.Tasks;
using System.Web.Owin.Server.Http;
using Microsoft.Owin.Builder;
using Nowin;
using Owin;

namespace System.Web.Owin.Server.Owin
{
    public class OwinHostedApplication : IHostedApplication
    {
        public IScope Scope { get; set; }
        protected Action<IScope> ScopeBuilder;
        

        public IHostedApplicationConfiguration Config { get; set; }
        public IApplication[] Applications { get; set; }
        public IHttpService Service { get; set; }
        

        protected static T AsIf<T>(object o) where T: class{
            return o.ActLike<T>();
        }

        public OwinHostedApplication(IHostedApplicationConfiguration config, params IApplication[] applications) {
            Config = config;
            Applications = applications;
            Scope = Provider.Get<IScope>();
            
        }

        public void Start() {
            var owinbuilder = new AppBuilder();
            OwinServerFactory.Initialize(owinbuilder.Properties);
            this.Configuration(owinbuilder);
            var builder = ServerBuilder.New();
            

            Config.If(c => !string.IsNullOrEmpty(c.HostName), c => builder.SetServerHeader(c.HostName));
            Config.If(c => c.Port <= 0, c => builder.SetPort(c.Port), c => builder.SetPort(8888));
            Config.If(c => !c.IPAddress.Equals(IPAddress.None), c => builder.SetAddress(c.IPAddress), c => builder.SetAddress(IPAddress.Any));

            builder
                .SetOwinApp(owinbuilder.Build())
                .SetOwinCapabilities(
                    (IDictionary<string, object>)owinbuilder.Properties[OwinKeys.ServerCapabilitiesKey]
                );
            var service = builder.Build();
            Service = AsIf<IHttpService>(service); //If it quacks like a duck....
        }

        public void Stop() {
            INowinServer service = Service;
            service.Dispose();
            Service = null;
        }

        public async void Configuration(IAppBuilder app) {
           
        }
    }
}
