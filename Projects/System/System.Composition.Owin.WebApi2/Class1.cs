using System;
using System.Collections.Generic;
using System.Composition.Autofac;
using System.Linq;
using System.Portable;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac.Integration.WebApi;
using Owin;

namespace System.Composition.Owin.WebApi2
{
    public static class AppBuilderExtensions {
        public static IAppBuilder ComposeWebApi(this IAppBuilder app) {
            app.Start();

            var config = new HttpConfiguration {
                DependencyResolver = 
                    new AutofacWebApiDependencyResolver(
                        Provider.Dependencies
                        .As<AutofacDependencyProvider>()
                        .Container.BeginLifetimeScope()
                    )
            };
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }
    }
}
