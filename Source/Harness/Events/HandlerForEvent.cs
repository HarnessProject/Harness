using System;

namespace Harness.Events {
    internal class HandlerForEvent<T> : IHandleEvent<T>
    {
        //Dear Microsoft, I love code analysis... But why this? I can make an event of Action<T>, so I'm gonna. --Nick
        //[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event EventHandler<T> Triggered;

        public Type EventType { get; private set; }
        
        public HandlerForEvent(EventHandler<T> handler) {
            Triggered += handler;
            EventType = typeof(T);
        }

        public void Handle(T eventObject)
        {
            if (Triggered != null) Triggered(eventObject);
        }


    }

    
}