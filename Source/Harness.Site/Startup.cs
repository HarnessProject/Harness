using System.Composition;
using Harness.Owin;
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
            
            app.UseHarness(x => {
                
            });
        }
    }
}
