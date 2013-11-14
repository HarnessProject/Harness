using System;
using Microsoft.Owin;

namespace Harness.Site.Server.Events {
    public class OwinPipelineEvent : ILoggedEvent
    {

        public string EventName { get; set; }
        public DateTime Timestamp { get; set; }
        public IOwinContext Context { get; set; }
        public string LogMessage { get; set; }

    }

   

    public interface ILoggedEvent {
        DateTime Timestamp { get; }
        string EventName { get; }
        string LogMessage { get; }
    }
}