using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harness.Autofac;
using Harness.Owin;
using Harness.Site.Server.Middleware;
using Microsoft.Owin.Hosting;
using Owin;

namespace Harness.Site.Server
{
    public class Program
    {
        public static void Main(string[] args) {
            using (WebApp.Start<Startup>("http://localhost:12345"))
            {
                Console.ReadLine();
            }
        }
    }

    public class Startup {
        public void Configuration(IAppBuilder app) {
            app.UseHarness(
                new Net.Environment<IDependency>(),
                new AutofacApplicationFactory()
            );
        }
    }
}
