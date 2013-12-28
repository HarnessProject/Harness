using System;
using System.Collections.Generic;
using System.Composition;
using System.Events;
using System.Linq;
using System.Portable.Events;
using System.Portable.Runtime;
using System.Runtime.CompilerServices;
using System.Runtime.Environment;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac;
using Autofac.Integration.Mvc;
using Harness.Autofac;
using Harness.Http;

namespace Harness.Web.Mvc {
    public class MvcContainerBuilder : IRegistrationProvider<ContainerBuilder>, IEventHandler {
        public async void Register(ITypeProvider typeProvider, ContainerBuilder builder) {
            await  
                typeProvider.Assemblies.AsParallel().ProcessAsync(
                    x => builder.RegisterControllers(x).InstancePerHttpRequest(),
                    x => builder.RegisterModelBinders(x).InstancePerHttpRequest()
                );
        }

        public async void HandleAppStart(ApplicationStartEvent e) {
            AreaRegistration.RegisterAllAreas();
            RouteTable.Routes.MapMvcAttributeRoutes();
            var scope = e.Scope;
            
            var routes = scope.Container.GetAllInstances<IRouteProvider>();
            var filters = scope.Container.GetAllInstances<IFilterProvider>();
            var bundles = scope.Container.GetAllInstances<IBundleProvider>();
            
            await routes.AsParallel().EachAsync(x => x.AddRoutes(RouteTable.Routes));
            await filters.AsParallel().EachAsync(x => x.AddFilters(GlobalFilters.Filters));
            await bundles.AsParallel().EachAsync(x => x.AddBundle(BundleTable.Bundles));

            DependencyResolver.SetResolver(new AutofacDependencyResolver(scope.Container.As<AutofacDependencyContainer>().Container));
        }

        
        public void Dispose()
        {
            ReleaseHandle();
        }

        public IStrongBox Handle { get; set; }
        public void ReleaseHandle()
        {
            Handle = null;
        }

        public IDictionary<IStrongBox, EventPipeline> Registrations { get; set; }
        public bool ShouldRegister(EventPipeline pipeline)
        {
            return true;
        }

        public void Register(EventPipeline pipeline)
        {
            pipeline.AddHandler<ApplicationStartEvent>(HandleAppStart);
        }

        public void UnRegister(EventPipeline pipeline)
        {
            throw new NotImplementedException();
        }
    }
}