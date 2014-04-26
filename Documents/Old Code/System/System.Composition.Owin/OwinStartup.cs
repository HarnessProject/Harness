using System;
using System.Collections.Generic;
using System.Portable;
using System.Portable.Runtime;
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
            
            Provider.GetAll<IApplication>().Each(a => a.Configure(app));

        }
    }
}
