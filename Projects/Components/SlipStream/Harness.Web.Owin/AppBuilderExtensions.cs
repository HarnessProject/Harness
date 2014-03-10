using System;
using System.Collections.Generic;
using System.Composition;
using System.Portable;
using System.Portable.Events;
using System.Portable.Runtime;
using System.Threading.Tasks;
using Autofac;
using Harness.Autofac;
using Harness.Http;
using Owin;

namespace Harness.Web.Owin {
    
    public static class AppBuilderExtensions {
        public static async Task<IAppBuilder> InitializeApp(this IAppBuilder app, Action<IScope> scopeFactory, ContainerBuilder builder = null) {

            await App.InitializeAsync(x => {
                x.Container = builder.NotNull() ? new AutofacDependencyProvider(builder.Build()) : new AutofacDependencyProvider(new HttpTypeProvider());
                scopeFactory(x);
            });
            var startEvent = new ApplicationStartEvent(app, App.Global);
            await App.EventManager.Trigger(startEvent);
            
            return app;
        }

        public static async Task<IAppBuilder> AddBuilder(this Task<IAppBuilder> app, Action<IAppBuilder> appAction) {
            var a = await app;
            appAction(a);
            return a;
        }

        public static IAppBuilder Use<T>(this IAppBuilder app) where T : IMiddleware {
            Exception e = null;
            bool r =
                app.Try(
                    x => {
                        x.Use(App.Container.Get<T>());       
                        return true;
                    })
                    .Catch<Exception>(
                        (x, ex) => {
                            e = ex;
                            return false;
                    })
                    .Act();

            if (!r) throw new Exception("Middleware " + typeof (T).FullName + " failed to load.", e);
            return app;
        }

        public static IAppBuilder Map<T>(this IAppBuilder app, string path = null) where T : IApplication {
            var m = App.Container.Get<T>();
            app.Map(path ?? m.BasePath, m.Configure);
            return app;
        }

        public static Task<IAppBuilder> MapAllAsync<T>(this IAppBuilder app) where T : IApplication {
            return app.Func(async a => {
                var m = App.Container.GetAll<T>();
                await m.EachAsync(x => app.Map(x.BasePath, x.Configure));
                return a;
            });
        }
    }
}