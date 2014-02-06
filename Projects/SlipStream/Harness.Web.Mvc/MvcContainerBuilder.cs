using System;
using System.Collections.Generic;
using System.Composition;
using System.Portable.Events;
using System.Linq;
using System.Portable.Runtime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Harness.Autofac;
using Harness.Http;

namespace Harness.Web.Mvc {
    public class MvcContainerBuilder : IRegistrationProvider<ContainerBuilder>, IEventHandler<ApplicationStartEvent> {
        protected List<Guid> RegisteredEventHandlers = new List<Guid>();
        public void Register(ITypeProvider typeProvider, ContainerBuilder builder) {
            
                typeProvider.Assemblies.AsParallel().Each(
                    x => builder.RegisterControllers(x).InstancePerHttpRequest(),
                    x => builder.RegisterModelBinders(x).InstancePerHttpRequest()
                );
        }

        public void Handle(ApplicationStartEvent e) {
            AreaRegistration.RegisterAllAreas();
            RouteTable.Routes.MapMvcAttributeRoutes();
            var scope = e.Scope;
            
            var routes = scope.Container.GetAll<IRouteProvider>();
            var filters = scope.Container.GetAll<IFilterProvider>();
            var bundles = scope.Container.GetAll<IBundleProvider>();
            
            routes.AsParallel().Each(x => x.AddRoutes(RouteTable.Routes));
            filters.AsParallel().Each(x => x.AddFilters(GlobalFilters.Filters));
            bundles.AsParallel().Each(x => x.AddBundle(BundleTable.Bundles));

            DependencyResolver.SetResolver(new AutofacDependencyResolver(scope.AutofacContainer()));
        }

        public void Dispose() {
            
        }
    }
}