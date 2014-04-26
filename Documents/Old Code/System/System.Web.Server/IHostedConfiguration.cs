using System.Linq;

namespace System.Web.Server {
    public interface IHostedApplicationConfiguration {
        string HostName { get; set; }
        int Port { get; set; }
        string IPAddress { get; set; }
    }

    
}