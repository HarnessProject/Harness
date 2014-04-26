//using Microsoft.Owin;
using System.Collections.Generic;
using System.Composition;
using System.Portable;
using Owin;

namespace System.Owin
{
    public class Startup
    {
        public void Configuration(IAppBuilder app) {
            
            Provider.GetAll<IApplication>().Each(a => a.Configure(app));

        }
    }
}
