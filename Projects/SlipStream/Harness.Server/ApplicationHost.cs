using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Tasks;
using System.Text;
using System.Threading.Tasks;
using Harness.Server.Http;
using Harness.Server.Owin;
using Harness.Web.Owin;
using ImpromptuInterface;

namespace Harness.Server
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
            application.AsTask(x => x.StartService());
        }

        IEnumerator<IHostedApplication> IEnumerable<IHostedApplication>.GetEnumerator() {
            return Applications.GetEnumerator();
        }
        public IEnumerator GetEnumerator() {
            return Applications.GetEnumerator();
        }
    }
}
