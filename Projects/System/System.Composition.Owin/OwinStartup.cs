using System;
using System.Portable;
using System.Portable.Reflection;
using System.Threading.Tasks;
//using Microsoft.Owin;
using Owin;
using System.Reactive.Linq;
using Owin.Types;



namespace System.Composition.Owin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app) {
            app = app.InitializeApp(x => {}).AwaitResult();
            Container.Get<IApplication>().Configure(app);

        }
    }
}
