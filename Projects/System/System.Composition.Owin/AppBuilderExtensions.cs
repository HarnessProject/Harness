using System.Composition.Autofac;
using System.Portable;
using System.Portable.Events;
using System.Portable.Runtime;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Autofac;
//using Microsoft.Owin;
using Owin;
using Owin.Types;

namespace System.Composition.Owin {
    
    public static class AppBuilderExtensions {
        public static async Task<IAppBuilder> InitializeApp(this IAppBuilder app, Action<IScope> scopeFactory, ContainerBuilder builder = null) {

            await App.InitializeAsync(x => {
                x.Container = builder.NotNull() ? new AutofacDependencyProvider(builder.Build()) : new AutofacDependencyProvider(new TypeProvider(null));
                scopeFactory(x);
            });
            var startEvent = new ApplicationStartEvent(app, App.Global);
            App.EventManager.OnNext(startEvent);
            
            return app;
        }

        public static async Task<IAppBuilder> AddBuilder(this Task<IAppBuilder> app, Action<IAppBuilder> appAction) {
            var a = await app;
            appAction(a);
            return a;
        }

        public static IAppBuilder Use<T>(this IAppBuilder app) where T : IMiddleware {
            Exception e = null;
            var r =
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

            if (!r) throw new ExternalException("Middleware failed to load.",e);
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

        public static IObservable<OwinRequest> AsObservable(this IAppBuilder app) {
            var r = new Reactive<OwinRequest>(default(OwinRequest));
            app.UseHandlerAsync(async (request, next) => {
                await r.SetValueAsync(request);
            });
            return r;
        }

    }
}