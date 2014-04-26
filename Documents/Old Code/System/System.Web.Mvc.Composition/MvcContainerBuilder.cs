using System.Collections.Generic;
using System.Composition;
using System.Composition.Autofac;
using System.Composition.Dependencies;
using System.Composition.Events;
using System.Composition.Providers;
using System.Linq;
using System.Portable;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.Mvc;

namespace System.Web.Mvc.Composition {
    public class MvcDependencyRegistrar : IRegisterDependencies, IEventHandler<ApplicationStartEvent> {
        protected List<Guid> RegisteredEventHandlers = new List<Guid>();
        public void Register(ITypeProvider typeProvider, IDependencyRegistrar builder) {
            var b = builder.As<AutofacDependencyRegistrar>().Builder;
            typeProvider.Assemblies.AsParallel().Each(
                x => b.RegisterControllers(x).InstancePerHttpRequest(),
                x => b.RegisterModelBinders(x).InstancePerHttpRequest()
            );
        }

        public void Handle(ApplicationStartEvent e) {
            AreaRegistration.RegisterAllAreas();
            RouteTable.Routes.MapMvcAttributeRoutes();
            var scope = e.Scope;
            
            var routes = Provider.GetAll<IRouteProvider>();
            var filters = Provider.GetAll<IFilterProvider>();
            var bundles = Provider.GetAll<IBundleProvider>();
            
            routes.AsParallel().Each(x => x.AddRoutes(RouteTable.Routes));
            filters.AsParallel().Each(x => x.AddFilters(GlobalFilters.Filters));
            bundles.AsParallel().Each(x => x.AddBundle(BundleTable.Bundles));

            DependencyResolver.SetResolver(new AutofacDependencyResolver(scope.AutofacContainer()));
        }

        public void Dispose() {
            
        }
    }
}