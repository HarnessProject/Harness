using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Autofac.Integration.SignalR;
using Harness.Autofac;
using Harness.Events;
using Harness.Framework;
using Microsoft.AspNet.SignalR;
using SignalrResolver = Microsoft.AspNet.SignalR.IDependencyResolver;
using Mvc = Autofac.Integration.Mvc;
using WebApi = Autofac.Integration.WebApi;
using Signalr = Autofac.Integration.SignalR;

namespace Harness.Web {

    public class WebContainerBuilder : IContainerBuilderService, IHandleEvent<ApplicationStartEvent> {
        public void AttachToBuilder(IEnvironment environment, ContainerBuilder builder) {
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

            var routes = e.Scope.ServiceLocator.GetAllInstances<IRouteProvider>();
            var filters = e.Scope.ServiceLocator.GetAllInstances<IFilterProvider>();
            var configs = e.Scope.ServiceLocator.GetAllInstances<IHttpConfigProvider>();
            var bundles = e.Scope.ServiceLocator.GetAllInstances<IBundleProvider>();
            
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


    }

    public class ApplicationStartEvent {
        
        public IScope Scope { get; set; }
    }

    public class Environment<T> : Net.Environment<T> {
        // This is safe, and not raised again by Code Analysis: Disable Warning... 
        // ReSharper disable once StaticFieldInGenericType
        private static readonly ContainerBuilder Builder = new ContainerBuilder();


        public Environment()
            : base() {
            
            
            
        }

        public override async Task<IEnumerable<Type>> GetTypes(string extensionsPath = null) {
            return await base.GetTypes(extensionsPath ?? HostingEnvironment.ApplicationVirtualPath + " \\bin");
        }
    }

    //MVC
    public interface IController : System.Web.Mvc.IController, IDependency {
        IScope LocalScope { get; set; }

    }

    //WebApi
    public interface IApiController : System.Web.Http.Controllers.IHttpController,IDependency {
        IScope LocalScope { get; set; }

    }

    //Routes, Filters, ApiConfig
    public interface IRouteConstraint : System.Web.Routing.IRouteConstraint, IDependency {}

    public interface IRouteHandler : System.Web.Routing.IRouteHandler, IDependency {}

    public interface IAreaRouteHandler : IRouteWithArea, IDependency {}

    public interface IRouteProvider : IDependency {
        void AddRoutes(RouteCollection routes);
    }

    public interface IFilterProvider : IDependency {
        void AddFilters(GlobalFilterCollection filters);
    }

    public interface IHttpConfigProvider : IDependency {
        void Configure(HttpConfiguration config);
    }

    public interface IBundleProvider : IDependency {
        void AddBundle(BundleCollection bundles);
    }
}