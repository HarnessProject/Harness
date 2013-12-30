using System.Events;
using Microsoft.Owin;

namespace Harness.Server.Owin.Events
{
    public class RequestReceivedEvent : Event<IOwinContext> {}

}
