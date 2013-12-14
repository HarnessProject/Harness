using System;
using System.Collections.Generic;
using System.Composition;
using System.Events;
using System.Linq;
using System.Runtime.Environment;
using System.Tasks;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Integration.SignalR;
using Harness.Autofac;
using Microsoft.AspNet.SignalR;

namespace Harness.Web.Signalr {

    

    public class SignalrComponentRegistrationService : IContainerBuilderService, IHandle<ApplicationStartEvent> {
        

        public async void Handle(ApplicationStartEvent tEvent) {
            var scope = tEvent.Parameter;
            
            await scope.Container
                .GetImplementationAsync<AutofacServiceLocator>(
                    x => GlobalHost.DependencyResolver = new AutofacDependencyResolver(x.Container)
                );
        }

        public async void AttachToBuilder(ITypeProvider typeProvider, ContainerBuilder builder) {
            await typeProvider.Assemblies.AsParallel().EachAsync(
                x => builder.RegisterHubs(x).ExternallyOwned()
            );
        }
    }
}
