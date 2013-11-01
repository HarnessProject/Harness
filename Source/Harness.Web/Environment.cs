using System;
using System.Linq;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Autofac.Integration.WebApi;
using Harness.Framework;

namespace Harness.Web {
    public class Environment<T> : Net.Environment<T> {
        // This is safe, and not raised again by Code Analysis: Disable Warning... 
        // ReSharper disable once StaticFieldInGenericType
        private static readonly ContainerBuilder Builder = new ContainerBuilder();

        public Environment(params Action<ContainerBuilder>[] builders)
            : base(false, () => Builder) {
            builders.AsParallel().EachAsync(x => x(Builder)).Await();

            AssemblyCache.AsParallel().ProcessAsync(
                x => Builder.RegisterApiControllers(x),
                x => Builder.RegisterControllers(x),
                x => Builder.RegisterModelBinders(x),
                x => Builder.RegisterWebApiModelBinders(x)
            ).Await();

            SetContainer(Builder.Build());

            DependencyResolver.SetResolver(new AutofacDependencyResolver(Container));
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
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