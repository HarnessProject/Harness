using System;
using System.Collections.Generic;
using System.Composition;
using System.Events;
using System.Net;
using System.Runtime.Remoting;
using System.Tasks;
using System.Web.Http;
using Autofac.Integration.WebApi;
using Harness.Autofac;
using Harness.Http;
using Harness.Server.Http;
using Harness.Server.Owin;
using Harness.Server.Owin.Events;
using Harness.TinyMessenger;
using Harness.Web.Owin;
using Harness.Web.WebApi2;
using ImpromptuInterface;

using Microsoft.Owin;
using Microsoft.Owin.Builder;
using Microsoft.Owin.Extensions;
using Nowin;
using Owin;

namespace Harness.Server.Owin
{
    public class OwinHostedApplication : IHostedApplication
    {
        public IScope Scope { get; set; }
        protected Action<IScope> ScopeBuilder;
        public IScope NewScope {
            get {
                var scope = new Scope();
                ScopeBuilder(scope);
                return scope;
            }
        }

        public IHostedApplicationConfiguration Config { get; set; }
        public IApplication[] Applications { get; set; }
        public IHttpService Service { get; set; }
        

        protected static T AsIf<T>(object o) where T: class{
            return o.ActLike<T>();
        }

        public OwinHostedApplication(IHostedApplicationConfiguration config, params IApplication[] applications) {
            Config = config;
            Applications = applications;
            ScopeBuilder = x => {
                x.Container = new AutofacDependencyProvider(new HttpTypeProvider());
                x.MessengerHub = new TinyMessengerHub();
                x.EventMessenger = new EventMessenger(x.MessengerHub, x);
            };
            Scope = NewScope;
        }

        public void Start() {
            var owinbuilder = new AppBuilder();
            OwinServerFactory.Initialize(owinbuilder.Properties);
            this.Configuration(owinbuilder);
            var builder = ServerBuilder.New();
            

            Config.When(c => !string.IsNullOrEmpty(c.HostName), c => builder.SetServerHeader(c.HostName));
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
            INowinServer service = Service.UndoActLike().As<INowinServer>();
            service.Dispose();
            Service = null;
        }

        public async void Configuration(IAppBuilder app) {
            Scope.State.Add("HttpConfig", new Action<HttpConfiguration>((config) =>
            {
                config.MapHttpAttributeRoutes();
                var configs = Scope.Container.GetAllInstances<IHttpConfigProvider>();

                config.Initializer = async c =>
                {
                    await configs.EachAsync(x => x.Configure(c));
                };

                config.DependencyResolver =
                    new AutofacWebApiDependencyResolver(Scope.Container.As<AutofacDependencyContainer>().Container);

            }));

            HttpConfiguration webApiConfig = Scope.State().HttpConfig(new HttpConfiguration());
            
            await app
            .AttachScope(Scope)
            
            .WithScope((a,s) => a.Use((context, previous) => {
                s.EventManager.Trigger(new RequestReceivedEvent {Parameter = context});
                return previous();
            }))
            .AddBuilder(async x => {
                x.UseWebApi(webApiConfig);
                await Applications.EachAsync(y => y.Configure(x));
            });
        }
    }
}
