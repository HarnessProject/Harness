using System;
using System.Collections.Generic;
using System.Events;
using System.Linq;
using System.Runtime.Environment;
using System.Tasks;
using System.Threading.Tasks;
using System.Web.Http;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Harness.Autofac;
using Owin;
using Harness.Http;


namespace Harness.Owin {
    

   


    public static class AppBuilderExtensions {
        public static async void UseHarness(this IAppBuilder app, Action<X> scopeFactory, ContainerBuilder builder = null) {

            await X.InitializeAsync(x => {
                x.Container = new AutofacServiceLocator(builder.Build());
                scopeFactory(x);
            });
            var startEvent = new ApplicationStartEvent(X.GlobalScope, new Dictionary<string, object> {{"scope", X.GlobalScope}});
            X.GlobalScope.MessengerHub.Publish(startEvent);
        }

        public static IAppBuilder Use<T>(this IAppBuilder app) where T : IMiddleware {
            Exception e = null;
            bool r =
                app.Try(
                    x => {
                        x.Use(X.GlobalScope.Container.GetInstance<T>());       
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
            var m = X.GlobalScope.Container.GetInstance<T>();
            app.Map(path ?? m.BasePath, m.Configure);
            return app;
        }

        public static Task<IAppBuilder> MapAllAsync<T>(this IAppBuilder app) where T : IApplication {
            return app.Func(async a => {
                var m = X.GlobalScope.Container.GetAllInstances<T>();
                await m.EachAsync(x => app.Map(x.BasePath, x.Configure));
                return a;
            });
        }
    }
}