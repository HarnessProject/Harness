using System;
using System.Collections.Generic;
using Autofac;
using Harness.Core;
using Harness.Framework;
using Harness.Net;
using Owin;

namespace Harness.OWIN {
    public static class AppBuilderExtensions {
        public static void UseHarness<T>(this IAppBuilder app, T environment, Func<ContainerBuilder> builder = null) {
            Func<ContainerBuilder> b = () => {
                var x = builder != null ? builder() : new ContainerBuilder();
                return x;
            };

            Application<Environment<IDependency>>.New(
                new Environment<IDependency>(true, b)
            );
            
            app.Properties.Add("Harness", typeof (Environment<IDependency>).FullName);

            //if (!autoLoad) return; // You could do this, but I wouldn't...
            //Application.Resolve<IEnumerable<IApplication>>().EachAsync(x => x.Configure(app)).Await();
            //Application.Resolve<IEnumerable<IMiddleware>>().EachAsync(x => app.Use(x)).Await();
        }

        public static IAppBuilder Use<T>(this IAppBuilder app) where T : IMiddleware {
            Exception e = null;
            bool r =
                app.Try(x => {
                            var m = Application.Resolve<T>();
                            x.Use(m);
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