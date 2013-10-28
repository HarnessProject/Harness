using System;
using System.Collections.Generic;
using System.Linq;

namespace Harness.Events {
    public interface IHandleEvent<in T> : IDependency {
        void Handle(T eventObject);
    }

    public interface IEventManager : IDependency {
        void Handle<T>(params Action<T>[] handlers);
        void Trigger<T>(T eventObject);
    }

    //public abstract class HandleEvent<T> : IHandleEvent<T>
    //{
    //    public event Action<dynamic> Triggered;

    //    protected HandleEvent(Action<dynamic> handler)
    //    {
    //        Triggered += handler;
    //    }

    //    public void Handle(T eventObject)
    //    {
    //        Triggered(eventObject);
    //    }
    //}

    public class EventManager : IEventManager {
        private readonly List<KeyValuePair<Type, Action<dynamic>>> _handlers =
            new List<KeyValuePair<Type, Action<dynamic>>>();

        #region IEventManager Members

        public void Handle<T>(params Action<T>[] handlers) {
            foreach (var handler in handlers)
                _handlers.Add(new KeyValuePair<Type, Action<dynamic>>(typeof (T), handler as Action<dynamic>));
        }

        public void Trigger<T>(T eventObject) {
            var handlers = new List<IHandleEvent<T>>();

            handlers.AddRange(Application.Resolve<IEnumerable<IHandleEvent<T>>>());

            foreach (var handler in handlers)
                handler.Handle(eventObject);

            foreach (var handler in _handlers.ToLookup(k => k.Key, v => v.Value)[typeof (T)])
                handler(eventObject);
        }

        #endregion
    }
}