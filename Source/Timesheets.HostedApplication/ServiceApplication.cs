using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Runtime.Environment;
using System.Tasks;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Core;
using Harness.Autofac;
using Harness.Server;
using Harness.Server.Owin;
using Harness.Web.Owin;
using Nancy;
using Nancy.Bootstrappers.Autofac;
using Nancy.Responses;
using Nancy.ViewEngines;
using Owin;

namespace Timesheets.HostedApplication
{
    public class ServiceApplication : OwinHostedApplication, IApplication  {
        public ServiceApplication(IHostedApplicationConfiguration config) : base(config, new IApplication[0]) {
            Applications = new IApplication[] {this};
            
        }

        public string BasePath { get { return ""; }}
        public async void Configure(IAppBuilder app) {

            await app
            .WithScope((oapp, scope) => oapp.UseNancy(o => {
                o.Bootstrapper = new NancyBootstrapper(scope);
            }));

        }
    }

    public class ApplicationRegistrationProvider : IRegistrationProvider<ContainerBuilder> {
        public async void Register(ITypeProvider typeProvider, ContainerBuilder builder) {
            var types = typeProvider.Types.ToArray().AsParallel();
            
            

            await types.Where(t => t.Is<INancyModule>()).EachAsync(
                x => builder.RegisterType<INancyModule>().InstancePerLifetimeScope()
            );
            await types.Where(t => t.Is<INancyEngine>()).EachAsync(
                x => builder.RegisterType<INancyEngine>().InstancePerLifetimeScope()
            );
        }
    }

    public class DiagModule : NancyModule {
        public DiagModule(IScope scope) {
            Before += (ctx, ct) => {
                ctx.Response.ContentType = "text/json";
                return null;
            };
            Get["/diagnostics",true] = async (p, ct) => {
                var result = await this.AsTask(x => new {
                        Message = "Hello World!",
                        Intent = "Harness Server is hosting NancyFx!"
                });

                return (Response) new JsonResponse(result, new DefaultJsonSerializer());
            };
        }
    }
    

    public sealed class NancyBootstrapper : AutofacNancyBootstrapper {
        private readonly IScope _scope;
        public NancyBootstrapper(IScope scope) : base() {
            _scope = scope;
            var b = new ContainerBuilder();
            b.RegisterInstance(_scope.CreateChildScope()).SingleInstance();
            b.Update(_scope.AutofacContainer());
        }

        protected override ILifetimeScope GetApplicationContainer() {
            return _scope.AutofacContainer();
        }
    }

}
