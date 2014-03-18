//using Microsoft.Owin;

using System.Portable;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Owin;
using Owin.Types;
using System.Reactive;

namespace System.Owin {
    
    public static class AppBuilderExtensions {
        public static IAppBuilder Start(this IAppBuilder app) {
            //Starts the Portable Runtime.
            Provider.Start(new FrameworkEnvironment());
            
            return app;
        }


        public static IAppBuilder Use<T>(this IAppBuilder app) where T : IMiddleware {
            Exception e = null;
            var r =
                app.Try(
                    x => {
                        x.Use(Provider.Get<T>());
                        return true;
                    })
                    .Catch<Exception>(
                        (x, ex) => {
                            e = ex;
                            return false;
                        })
                    .Act();

            if (!r) throw new ExternalException("Middleware failed to load.", e);
            return app;
        }

        public static IObservable<OwinHandlerContext> AsObservable(this IAppBuilder app, IReactive<OwinHandlerContext> source = null) {
            var r = source ?? new Reactive<OwinHandlerContext>(default(OwinHandlerContext));
            app.UseHandlerAsync(async (request, response, next) => await r.SetValueAsync(new OwinHandlerContext { Request = request, Response = response, Next = next}));
            return r;
        }

    }

    public class OwinHandlerContext {
        public OwinRequest Request { get; set; }
        public OwinResponse Response { get; set; }
        public Func<Task> Next { get; set; } 
    }
}