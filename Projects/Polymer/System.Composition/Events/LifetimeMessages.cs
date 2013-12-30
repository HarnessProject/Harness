using System.Composition;
using System.Contracts;
using System.Threading;

namespace System.Events
{
    
    public delegate void EventMessageHandler<T>(Event<T> message);

    public delegate void EventMessageHandler(Event message );


    public abstract class Event : IEvent {
        public object Sender { get;  set; }
        public string Name { get;  set; }
        public string Description { get;  set; }
        public DateTime TimeStamp { get;  set; }
        public IEvent Parent { get;  set; }
        public object Parameter { get;  set; }
        public CancellationToken Canceled { get; set; }
    }

    public abstract class Event<T> : IEvent<T>
    {
        public T Parameter { get;  set; }
        public CancellationToken Canceled { get; set; }

        public DateTime TimeStamp { get;  set; }
        public IEvent Parent { get;  set; }
        public object Sender { get;  set; }
        public string Name { get;  set; }
        
        public string Description { get;  set; }
        object IEvent.Parameter { get { return Parameter; } }
    }

    public abstract class ApplicationEvent<T> : Event<T>
    {
        protected ApplicationEvent(object sender, T parameter, IEvent parent = null)
        {
            Sender = sender;
            Parameter = parameter;
            TimeStamp = DateTime.Now;
            Parent = parent;
        }

    }

    public class ApplicationStartEvent : ApplicationEvent<IScope> {
        public ApplicationStartEvent(object sender, IScope scope) : base(sender, scope) {
            Name = "Application Start";
            Description = "The Application has started";

        }
    }
    
}
