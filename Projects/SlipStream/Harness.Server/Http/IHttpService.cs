using System.Events;
using Harness.Server.Owin.Events;

namespace Harness.Server.Http
{
    public interface IHttpService : INotify<RequestReceivedEvent> {
        int ConnectionCount { get; }
        int CurrentMaxConnectionCount { get; }
        void Start();
    }
}
