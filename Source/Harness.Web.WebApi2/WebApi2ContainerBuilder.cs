using System;
using System.Composition;
using System.Events;
using System.Linq;
using System.Runtime.Environment;
using System.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.WebApi;
using Harness.Autofac;
using Harness.Http;

namespace Harness.Web.WebApi2 {
    public class WebApi2ContainerBuilder : IContainerBuilderService, IHandle<ApplicationStartEvent> {

        public async void AttachToBuilder(ITypeProvider environment, ContainerBuilder builder) {
            await environment.Assemblies.AsParallel().ProcessAsync(
                x => builder.RegisterApiControllers(x).InstancePerApiRequest(),
                x => builder.RegisterWebApiModelBinders(x).InstancePerApiRequest()
            );
        }

        public async void Handle(ApplicationStartEvent e) {
            var scope = e.Parameter;
            var configs = scope.Container.GetAllInstances<IHttpConfigProvider>();
            var config = new HttpConfiguration {
                Initializer = async c => {
                    await configs.EachAsync(x => x.Configure(c));
                }
            };
            await scope.Container
                .GetImplementationAsync<AutofacServiceLocator>(
                    x => config.DependencyResolver = new AutofacWebApiDependencyResolver(x.Container));

            config.MapHttpAttributeRoutes();
            scope.State.Add("HttpConfig", config);
        }
    }
}