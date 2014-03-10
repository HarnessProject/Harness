using System;
using System.Contracts;
using System.Portable.Events;
using Microsoft.Owin;

namespace Harness.Server.Owin.Events
{
    public class RequestReceivedEvent : IEvent {
        public object Sender { get; private set; }
        public string Title { get; private set; }
        public string Description { get; private set; }
        public DateTime TimeStamp { get; private set; }
        public IEvent Parent { get; private set; }
        public ICancelToken Token { get; private set; }
    }

}
