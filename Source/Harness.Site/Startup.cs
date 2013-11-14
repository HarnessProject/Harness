using System.Security.Cryptography.X509Certificates;
using Autofac.Integration.SignalR;
using Harness.OWIN;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Harness.Site.Startup))]
namespace Harness.Site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            app.UseHarness(
                new Web.Environment<IDependency>(
                    x => x.RegisterHubs()
                )
            );
        }
    }
}
