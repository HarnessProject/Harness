using System;
using System.Composition;
using System.Events;
using System.Linq;
using System.Runtime.Environment;
using System.Security.Cryptography.X509Certificates;
using System.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Core;
using Autofac.Integration.WebApi;
using Harness.Autofac;
using Harness.Http;
using System.Web.Http;

namespace Harness.Web.WebApi2 {
    public class WebApiReadyEvent : Event {}

    public class WebApi2ContainerBuilder : IRegistrationProvider<ContainerBuilder>, IHandle<ApplicationStartEvent> {

        public async void Register(ITypeProvider environment, ContainerBuilder builder) {
            await environment.Assemblies.AsParallel().ProcessAsync(
                x => builder.RegisterApiControllers(x).InstancePerApiRequest(),
                x => builder.RegisterWebApiModelBinders(x).InstancePerApiRequest()
               
            );
        }


        public async void Handle(ApplicationStartEvent e) {

            var scope = e.Parameter;
            scope.State.Add("HttpConfig", new Action<HttpConfiguration>((config) => {
                config.MapHttpAttributeRoutes();
                var configs = scope.Container.GetAllInstances<IHttpConfigProvider>();
            
                config.Initializer = async c => {
                    await configs.EachAsync(x => x.Configure(c));
                };
              
                config.DependencyResolver = 
                    new AutofacWebApiDependencyResolver(scope.Container.As<AutofacDependencyContainer>().Container);

            }));

            await scope.EventMessenger.Trigger(new WebApiReadyEvent());
        }
    }
}