using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Autofac.Integration.SignalR;
using Harness.Framework;
using Microsoft.AspNet.SignalR;
using SignalrResolver = Microsoft.AspNet.SignalR.IDependencyResolver;
using Mvc = Autofac.Integration.Mvc;
using WebApi = Autofac.Integration.WebApi;
using Signalr = Autofac.Integration.SignalR;

namespace Harness.Web {
    public class Environment<T> : Net.Environment<T> {
        // This is safe, and not raised again by Code Analysis: Disable Warning... 
        // ReSharper disable once StaticFieldInGenericType
        private static readonly ContainerBuilder Builder = new ContainerBuilder();

        public Environment(params Action<ContainerBuilder>[] builders)
            : base(false, () => Builder) {
            builders.AsParallel().EachAsync(x => x(Builder)).Await();

            AssemblyCache.AsParallel().ProcessAsync(
                x => Builder.RegisterApiControllers(x).InstancePerApiRequest(),
                x => Builder.RegisterControllers(x).InstancePerHttpRequest(),
                x => Builder.RegisterModelBinders(x).InstancePerHttpRequest(),
                x => Builder.RegisterWebApiModelBinders(x).InstancePerApiRequest(),
                x => Builder.RegisterHubs(x).ExternallyOwned()
            ).Await();

            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            SetContainer(Builder.Build());
            
            DependencyResolver.SetResolver(new Mvc.AutofacDependencyResolver(Container));
            GlobalConfiguration.Configuration.DependencyResolver = new WebApi.AutofacWebApiDependencyResolver(Container);
            GlobalHost.DependencyResolver = new Signalr.AutofacDependencyResolver(Container.BeginLifetimeScope()).As<SignalrResolver>();
        }

        public override IEnumerable<Type> GetTypes(string extensionsPath = null) {
            return base.GetTypes(extensionsPath ?? HostingEnvironment.ApplicationPhysicalPath + " \\bin");
        }
    }

    //MVC
    public interface IController : System.Web.Mvc.IController, IDependency {}

    //WebApi
    public interface IApiController : IDependency {}

    //Routes, Filters, ApiConfig
    public interface IConstrainRoutes : IRouteConstraint, IDependency {}

    public interface IHandleRoutes : IRouteHandler, IDependency {}

    public interface IHandleAreaRoutes : IRouteWithArea, IDependency {}

    public interface IProvideRoutes : IDependency {
        void AddRoutes(RouteCollection routes);
    }

    public interface IProvideFilters : IDependency {
        void AddFilters(GlobalFilterCollection filters);
    }

    public interface IConfig : IDependency {
        void Configure(HttpConfiguration config);
    }
}