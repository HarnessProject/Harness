using System.Collections.Generic;
using System.Composition.Dependencies;
using System.Web.Owin.Server.Http;
using System.Web.Server;

namespace System.Web.Owin.Server {
    public interface IApplicationHost : IDependency {
        IList<IHostedApplication> Applications { get; set; }
        IList<IHttpService> Servers { get; }

        void Add(IHostedApplication application);
    }
}