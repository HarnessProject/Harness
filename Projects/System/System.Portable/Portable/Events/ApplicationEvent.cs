using System.Portable.Runtime;

namespace System.Portable.Events {
    public abstract class ApplicationEvent : Event {
        protected ApplicationEvent(object sender, IScope scope, IEvent parent = null) {
            Sender = sender;
            TimeStamp = DateTime.Now;
            Parent = parent;
            Scope = scope;
        }

        public IScope Scope { get; set; }
    }
}