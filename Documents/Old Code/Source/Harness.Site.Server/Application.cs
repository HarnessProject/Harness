using System;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Harness.Autofac;
using Harness.Framework;
using Harness.Net;
using Harness.Owin;
using Harness.Site.Server.Middleware;
using Microsoft.AspNet.SignalR;
using Owin;

namespace Harness.Site.Server
{
    public class AdministrationApplication : IServerApplication
    {
        public void Configure(IAppBuilder app) {
           
        }

        public string BasePath { get { return "admin"; } }

        
    }

    public class Server {
        public static Server Current { get; internal set; }
        internal IContainer Container { get { return Environment.Container; } }
        internal IEnvironment Environment { get; set; }
        internal Server() {}

        public async Task<ILifetimeScope> GetScopeAsync() {
            return await Container.AsTask().ActionAsync(x => x.BeginLifetimeScope());
        }
        
    }

    public static class AppBuilderExtensions {
        public static IAppBuilder StartServer(this IAppBuilder app, params Action<ContainerBuilder>[] builders)
        {
            app.Properties.Add("Harness.Site.Server", app.CreateServerAsync(builders).AwaitResult());
            return app;
        }
        
        public static async Task<IAppBuilder> UsingServerAsync<T>(this IAppBuilder app, Action<Server, ILifetimeScope> action) {
            var server = app.Properties["Harness.Site.Server"].As<Server>();
            var scope = await server.GetScopeAsync();
            await action.AsTask(x => x(server, scope));
            return app;
        }  

        internal static async Task<Server> CreateServerAsync(this IAppBuilder app, params Action<ContainerBuilder>[] builders) {

            X.Initialize(new AutofacApplicationFactory(){ BuildContainer = true });

            ILifetimeScope container = null;
            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(server.Container);
            GlobalHost.DependencyResolver = new AutofacDependencyResolver(server.Container.BeginLifetimeScope()).As<IDependencyResolver>();
            return server;
        }
    }
            

}