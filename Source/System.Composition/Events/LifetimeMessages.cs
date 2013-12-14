using System.Composition;
using System.Contracts;

namespace System.Events
{
    
    public delegate void EventMessageHandler<T>(EventMessage<T> message);

    public abstract class EventMessage<T> : IEvent<T>
    {
        public T Parameter { get; protected set; }
        public ICancelToken Token { get; protected set; }
        public DateTime TimeStamp { get; protected set; }
        public IEvent Parent { get; protected set; }
        public object Sender { get; protected set; }
        public string Name { get; protected set; }
        
        public string Description { get; protected set; }
        object IEvent.Parameter { get { return Parameter; } }
    }

    public abstract class ApplicationEvent : EventMessage<IScope>
    {
        protected ApplicationEvent(object sender, IScope scope, IEvent parent = null)
        {
            Sender = sender;
            Parameter = scope;
            TimeStamp = DateTime.Now;
            Parent = parent;
        }

    }

    public class ApplicationStartEvent : ApplicationEvent {
        public ApplicationStartEvent(object sender, IScope scope) : base(sender, scope) {
            Name = "Application Start";
            Description = "The Application has started";

        }
    }
    
}
