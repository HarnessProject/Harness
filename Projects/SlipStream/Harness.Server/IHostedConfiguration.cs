using System.Net;
using Harness.Web.Owin;

namespace Harness.Server {
    public interface IHostedApplicationConfiguration {
        string HostName { get; set; }
        int Port { get; set; }
        IPAddress IPAddress { get; set; }
        IApplication[] Applications { get; set; }
    }
}