using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Autofac;
using Harness.Core;
using Harness.Framework;
using Harness.Net;
using Microsoft.Owin;
using Owin;
using Owin.Dependencies.Autofac;

namespace Harness.OWIN {
    public static class AppBuilderExtensions {
        public static void UseHarness(this IAppBuilder app, Func<ContainerBuilder> builder = null) {
            Func<ContainerBuilder> b = () => {
                ContainerBuilder x = builder != null ? builder() : new ContainerBuilder();
                x.RegisterOwinApplicationContainer();
                return x;
            };

            Application<Environment<IDependency>>.New(new Environment<IDependency>(true, b));
            app.Properties.Add("Harness", typeof (Environment<IDependency>).FullName);
            app.UseAutofacContainer(Application.CurrentEnvironment.Container);
        }

        public static IAppBuilder Use<T>(this IAppBuilder app) where T : IMiddleware {
            Exception e = null;
            bool r =
                app.Try(
                    x => {
                        var m = Application.Resolve<T>();
                        x.Use(m);
                        return true;
                    }
                    ).Catch<Exception>(
                        (x, ex) => {
                            e = ex;
                            return false;
                        }).Invoke();

            if (!r) throw new Exception("Middleware " + typeof (T).FullName + "Failed to load.", e);

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

    public interface IApplication : IDependency {
        string BasePath { get; }
        void Configure(IAppBuilder app);
    }

    public interface IMiddleware : IDependency {
        Task Invoke(IOwinContext context, OwinMiddleware next);
    }

    public abstract class BaseMiddleWare : OwinMiddleware, IMiddleware {
        protected BaseMiddleWare(OwinMiddleware next) : base(next) {}

        public new OwinMiddleware Next { get; set; }

        #region IMiddleware Members

        public abstract Task Invoke(IOwinContext context, OwinMiddleware next);

        #endregion

        public override Task Invoke(IOwinContext context) {
            Invoke(context, Next);
        }
    }
}