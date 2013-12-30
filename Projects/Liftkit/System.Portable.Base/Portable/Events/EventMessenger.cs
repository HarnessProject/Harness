using System.Collections.Generic;
using System.Composition;
using System.Events;
using System.Linq;
using System.Linq.Expressions;
using System.Messaging;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Portable.Events
{

    public interface IEventManager {
        EventPipeline Create(string name);
        EventPipeline Get(string name);
    }

    public delegate void EventHandler(IEvent tEvent);
    public delegate void EventHandler<T>(T tEvent) where T: IEvent;
    public delegate bool EventFilter(IEvent tEvent);
    public delegate bool EventFilter<T>(T tEvent) where T : IEvent;

    public interface IEventHandler : IHold {
        IDictionary<IStrongBox, EventPipeline> Registrations { get; set; } 
        bool ShouldRegister(EventPipeline pipeline);
        void Register(EventPipeline pipeline);
        void UnRegister(EventPipeline pipeline);
    }

    public class EventPipeline {
        protected HashSet<RegisteredEventHandler> Handlers { get; set; }
        protected IStrongBox Handle { get; set; }

        public EventPipeline() {
            Handlers = new HashSet<RegisteredEventHandler>();
            Handle = new StrongBox<Object>(new object());
        }

        public IStrongBox AddHandler<T>(EventHandler<T> handler, EventFilter<T> filter = null) where T: IEvent {
            var compiledHandler = ((Expression<EventHandler>) ( x => handler(x.As<T>()) )).Compile();
            var compiledFilter = ((Expression<EventFilter>) ( x => filter.IsNull() || filter(x.As<T>()) )).Compile();
            var newHandler = new RegisteredEventHandler() {
                Pipeline = this,
                Id = Guid.NewGuid(),
                Filter = compiledFilter,
                Handler = compiledHandler,
                TargetType = typeof(T)
            };
            Handlers.Add(newHandler);
            return new StrongBox<Guid>(newHandler.Id);
        }


        protected void DefaultHandler(IEvent tEvent) {}

        public async Task Trigger(IEvent tEvent) {
            
            var h = 
                Handlers
                .Where(x => x.TargetType.Is(tEvent.GetType()))
                .Where(x => x.Filter(tEvent))
                .Aggregate(
                    new EventHandler(DefaultHandler),
                    (handler, eventHandler) => handler + delegate(IEvent evnt) { if (!evnt.Token.Canceled) eventHandler.Handler(evnt); });

            await Handle.AsTask(x => h(tEvent));
        }
    }

    
    public interface IProvideCallback<in T> {
        void Callback(T message);
    }

    public class RegisteredEventHandler {
        public EventPipeline Pipeline { get; set; }
        public Guid Id { get; set; }
        public Type TargetType { get; set; }
        public EventHandler Handler { get; set; }
        public EventFilter Filter { get; set; }
    }

    public class RegisteredEventFilter {
        public Type TargetType { get; set; }
    }
    


   
}
