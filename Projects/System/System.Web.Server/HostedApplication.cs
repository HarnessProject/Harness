using System.Collections.Generic;
using System.Composition;
using System.Reactive;
using System.Reactive.Subjects;
using System.Web.Owin.Server.Http;

namespace System.Web.Server
{
    public abstract class HostedApplication<T> : Reactive<T>, IHostedApplication {
        protected HostedApplication(IHostedApplicationConfiguration config) : base() {
            Config = config;
            
        }

        public IScope Scope { get; set; }
        public IHostedApplicationConfiguration Config { get; protected set; }
        public IHttpService Service { get; protected set; }
        public abstract void Start();
        public abstract void Stop();
    }



   

    
}
