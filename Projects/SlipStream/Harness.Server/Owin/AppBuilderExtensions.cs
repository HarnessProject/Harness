using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Tasks;
using System.Text;
using System.Threading.Tasks;
using ImpromptuInterface.Dynamic;
using Owin;

namespace Harness.Server.Owin
{
    public static class AppBuilderExtensions
    {
        public static IAppBuilder AttachScope(this IAppBuilder app, IScope scope) {
            app.Properties["HostedApplication.Scope"] = scope;
            return app;
        }

        public static async Task<IAppBuilder> WithScope(this IAppBuilder app, params Action<IAppBuilder, IScope>[] builders) {
            var scope = app.Properties["HostedApplication.Scope"].As<IScope>();
            await builders.EachAsync(x => x(app,scope));
            return app;
        }
    }
}
