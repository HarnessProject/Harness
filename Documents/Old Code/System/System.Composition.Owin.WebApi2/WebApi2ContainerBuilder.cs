using System.Collections.Generic;
using System.Composition.Autofac;
using System.Composition.Dependencies;
using System.Composition.Events;
using System.Composition.Providers;
using System.Linq;
using System.Portable.Events;
using System.Threading.Tasks;
using Autofac.Integration.WebApi;

namespace System.Composition.Owin.WebApi2 {
    public class WebApiReadyEvent : Event {}

    public class WebApi2ContainerBuilder : IRegisterDependencies {

        public void Register(ITypeProvider environment, IDependencyRegistrar builder) {
            var b = builder.As<AutofacDependencyRegistrar>().Builder;
            environment.Assemblies.AsParallel().Each(
                x => b.RegisterApiControllers(x).InstancePerApiRequest(),
                x => b.RegisterWebApiModelBinders(x).InstancePerApiRequest()
            );
        }


        

        public void Dispose() {
            
        }
    }
}