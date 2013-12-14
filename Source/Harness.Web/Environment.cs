using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Messaging;
using System.Runtime.Environment;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Harness.Autofac;
using Harness.Http;
using Microsoft.AspNet.SignalR;
using SignalrResolver = Microsoft.AspNet.SignalR.IDependencyResolver;
using Mvc = Autofac.Integration.Mvc;
using WebApi = Autofac.Integration.WebApi;
using Signalr = Autofac.Integration.SignalR;

namespace Harness.Web {

    public class WebContainerBuilder : IContainerBuilderService, IReceive<ApplicationStartEvent> {
        
        public void AttachToBuilder(ITypeProvider environment, ContainerBuilder builder) {
            X.Assemblies.AsParallel().ProcessAsync(
                x => builder.RegisterApiControllers(x).InstancePerApiRequest(),
                x => builder.RegisterControllers(x).InstancePerHttpRequest(),
                x => builder.RegisterModelBinders(x).InstancePerHttpRequest(),
                x => builder.RegisterWebApiModelBinders(x).InstancePerApiRequest(),
                x => builder.RegisterHubs(x).ExternallyOwned()
            ).Await();
        }

        public Type EventType { get { return typeof (ApplicationStartEvent); } }
        public async void Handle(ApplicationStartEvent e) {
            
            //Automatic for the people!
            AreaRegistration.RegisterAllAreas();
            RouteTable.Routes.MapMvcAttributeRoutes();
            GlobalConfiguration.Configuration.MapHttpAttributeRoutes();

           
            var configs = e.Scope.ServiceLocator.GetAllInstances<IHttpConfigProvider>();
           
            
            await routes.EachAsync(x => x.AddRoutes(RouteTable.Routes));
            await filters.EachAsync(x => x.AddFilters(GlobalFilters.Filters));
            await configs.EachAsync(x => GlobalConfiguration.Configure(x.Configure));
            await bundles.EachAsync(x => x.AddBundle(BundleTable.Bundles));

            ILifetimeScope container = null;
            X.ServiceLocator.GetImplementation<AutofacServiceLocator>(x => container = x.Container);

            DependencyResolver.SetResolver(new Mvc.AutofacDependencyResolver(container));
            GlobalConfiguration.Configuration.DependencyResolver = new WebApi.AutofacWebApiDependencyResolver(container);
            GlobalHost.DependencyResolver = new Signalr.AutofacDependencyResolver(container.BeginLifetimeScope()).As<SignalrResolver>();
        }

        public bool Deliver(ApplicationStartEvent message) {
            throw new NotImplementedException();
        }
    }

   

    //MVC
 

    //WebApi
  
}