using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Harness.Web.Mvc.Site.Startup))]
namespace Harness.Web.Mvc.Site
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
