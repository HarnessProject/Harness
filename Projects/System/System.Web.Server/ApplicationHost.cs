using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Owin.Server;
using System.Web.Owin.Server.Http;

namespace System.Web.Server
{
    public class ApplicationtHost : IApplicationHost, IEnumerable<IHostedApplication> {
        public ApplicationtHost()
        {
            Applications = new List<IHostedApplication>();
        }

        public IList<IHostedApplication> Applications { get; set; }
        public IList<IHttpService> Servers { get { return Applications.Select(x => x.Service).ToList(); }}
        
        public void Add(IHostedApplication application) {
            Applications.Add(application);
            application.AsTask(x => x.Start());
        }

        IEnumerator<IHostedApplication> IEnumerable<IHostedApplication>.GetEnumerator() {
            return Applications.GetEnumerator();
        }
        public IEnumerator GetEnumerator() {
            return Applications.GetEnumerator();
        }
    }
}
