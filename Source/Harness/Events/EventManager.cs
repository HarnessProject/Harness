using System;
using System.Collections.Generic;
using System.Linq;
using Harness.Framework;

namespace Harness.Events {
    public delegate void EventHandler(object eventObject);
    public delegate void EventHandler<in T>(T eventObject);

    public class EventManager : IEventManager {
        private readonly LookupTable<Type, EventHandler> _handlers;
        public EventManager() {
            _handlers = new LookupTable<Type, EventHandler>();
        }

        #region IEventManager Members

        public void Handle<T>(params Action<T>[] handlers) where T : class {
            _handlers.Add(
                typeof (T), 
                handlers
                    .Select<Action<T>,EventHandler>(x => y => x(y as T))
                    .ToArray()
            );
        }

        public void Trigger<T>(T eventObject) where T : class {
            var handlers = new List<IHandleEvent<T>>();
            var tType = typeof (T);
            
            handlers.AddRange(X.ServiceLocator.GetAllInstances<IHandleEvent<T>>());
            handlers.AddRange(
                _handlers
                    .ToList()
                    .Where(x => x.Key == tType)
                    .Select(
                        x => new HandlerForEvent<T>(y => x.Value(y))
                    )
            );

            foreach (var handler in handlers) 
                handler
                .AsTask()
                .ActionAsync(x => x.Handle(eventObject))
                .Begin();
        }

        #endregion
    }
}