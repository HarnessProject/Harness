using System.Composition;
using System.Contracts;
using System.Portable.Events;
using System.Portable.Runtime;
using System.Threading;

namespace System.Events
{
    
    

    public abstract class Event : IEvent {

        public object Sender
        {
            get; set;
        }

        public string Title
        {
            get; set;
        }

        public string Description
        {
            get; set;
        }

        public DateTime TimeStamp
        {
            get; set;
        }

        public IEvent Parent
        {
            get; set;
        }

        public ICancelToken Token
        {
            get; set;
        }
    }

   

    public abstract class ApplicationEvent<T> : Event
    {
        public IScope Scope { get; set; }
        protected ApplicationEvent(object sender, IScope scope, IEvent parent = null)
        {
            Sender = sender;
            TimeStamp = DateTime.Now;
            Parent = parent;
            Scope = scope;
        }

    }

    public class ApplicationStartEvent : ApplicationEvent<IScope> {
        
        public ApplicationStartEvent(object sender, IScope scope) : base(sender, scope) {
            Title = "Application Start";
            Description = "The Application has started";
            
        }
    }
    
}
