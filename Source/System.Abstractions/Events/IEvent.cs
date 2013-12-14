using System;
using System.Collections.Generic;
using System.Contracts;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading;

namespace System.Events
{
    public interface IEventMessenger
    {
        void Handle<T>(Action<T> handler) where T : class, IEvent;
        ICancelToken Trigger<T>(T evnt) where T : class, IEvent;
    }

    public interface IEvent : IMessage {
        string Name { get; }
        string Description { get; }
        DateTime TimeStamp { get; }
        IEvent Parent { get; } 
        object Parameter { get; }
        ICancelToken Token { get; }
    }

    public interface IEvent<T> : IEvent
    {
        new T Parameter { get; }  
    }

    public interface IHandle<T> where T: IEvent  {
        void Handle(T tEvent);
    }
}
