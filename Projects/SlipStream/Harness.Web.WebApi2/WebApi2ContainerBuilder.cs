using System;
using System.Composition;
using System.Linq;
using System.Portable.Events;
using System.Portable.Runtime;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Core;
using Autofac.Integration.WebApi;
using Harness.Autofac;
using Harness.Http;
using System.Web.Http;

namespace Harness.Web.WebApi2 {
    public class WebApiReadyEvent : Event {}

    public class WebApi2ContainerBuilder : IRegistrationProvider<ContainerBuilder>, IEventHandler<ApplicationStartEvent> {

        public async void Register(ITypeProvider environment, ContainerBuilder builder) {
            await environment.Assemblies.AsParallel().ProcessAsync(
                x => builder.RegisterApiControllers(x).InstancePerApiRequest(),
                x => builder.RegisterWebApiModelBinders(x).InstancePerApiRequest()
               
            );
        }


        public async void Handle(ApplicationStartEvent e) {

            var scope = e.Scope;
            scope.State.Add("HttpConfig", new Action<HttpConfiguration>((config) => {
                config.MapHttpAttributeRoutes();
                var configs = scope.Container.GetAll<IHttpConfigProvider>();
            
                config.Initializer = async c => {
                    await configs.EachAsync(x => x.Configure(c));
                };
              
                config.DependencyResolver = 
                    new AutofacWebApiDependencyResolver(scope.AutofacContainer());

            }));

            await scope.EventManager.Trigger(new WebApiReadyEvent());
        }

        public void Dispose() {
            
        }
    }
}