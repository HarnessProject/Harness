using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Harness.Autofac;
using Harness.Events;
using Harness.Framework;
using Harness.Net;
using Microsoft.AspNet.SignalR;
using Owin;
using System.Web.Http;


namespace Harness.Owin {
    public class ApplicationStartEvent
    {
        public IScope Scope { get; set; }
    }
    public static class AppBuilderExtensions {
        public static void UseHarness<T>(this IAppBuilder app, T environment, IApplicationFactory factory) where T : IEnvironment {
           
            X.Initialize(factory, environment);
            X.ServiceLocator.GetInstance<IEventManager>().Trigger(new ApplicationStartEvent() { Scope = X.GlobalScope });
        }

        public static IAppBuilder Use<T>(this IAppBuilder app) where T : IMiddleware {
            Exception e = null;
            bool r =
                app.Try(
                    x => {
                        app.Use(X.ServiceLocator.GetInstance<T>());       
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
            var m = X.ServiceLocator.GetInstance<T>();
            app.Map(path ?? m.BasePath, m.Configure);
            return app;
        }

        public static IAppBuilder MapAll<T>(this IAppBuilder app) where T : IApplication {
            var m = X.ServiceLocator.GetAllInstances<T>();
            m.EachAsync(x => app.Map(x.BasePath, x.Configure)).Await();
            return app;
        }
    }
}