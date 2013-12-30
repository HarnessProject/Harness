using System.Collections.Generic;
using System.Composition;
using Harness.Server.Http;

namespace Harness.Server {
    public interface IApplicationHost : IDependency {
        IList<IHostedApplication> Applications { get; set; }
        IList<IHttpService> Servers { get; }

        void Add(IHostedApplication application);
    }
}