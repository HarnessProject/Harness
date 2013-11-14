using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Harness.Core;
using Harness.Framework;
using Harness.Net;
using Microsoft.AspNet.SignalR;
using Owin;
using System.Web.Http;


namespace Harness.Owin {
    public static class AppBuilderExtensions {
        public static void UseHarness<T>(this IAppBuilder app, T environment, Func<ContainerBuilder> builder = null) {
            var b = builder != null ? builder() : new ContainerBuilder();
            
            var server = Application<Environment<IDependency>>.New(
                new Environment<IDependency>(true, () => b)
            );
            
            b.RegisterInstance(app).AsSelf();
            server.Environment.SetContainer(b.Build());
            app.Properties.Add("Harness", server);

        }

        public static Harness.IApplication GetHarness(this IAppBuilder app) {
            return app.Properties["Harness"].As<Harness.IApplication>();
        }

        public static IAppBuilder Use<T>(this IAppBuilder app) where T : IMiddleware {
            Exception e = null;
            bool r =
                app.Try(
                    x => {
                        app.Use(Application.Resolve<T>());       
                        return true;
                    })
                    .Catch<Exception>(
                        (x, ex) => {
                            e = ex;
                            return false;
                        })
                    .Invoke();

            if (!r) throw new Exception("Middleware " + typeof (T).FullName + " failed to load.", e);
            return app;
        }

        public static IAppBuilder Map<T>(this IAppBuilder app, string path = null) where T : IApplication {
            var m = Application.Resolve<T>();
            app.Map(path ?? m.BasePath, m.Configure);
            return app;
        }

        public static IAppBuilder MapAll<T>(this IAppBuilder app) where T : IApplication {
            var m = Application.Resolve<IEnumerable<T>>();
            m.EachAsync(x => app.Map(x.BasePath, x.Configure)).Await();
            return app;
        }
    }
}