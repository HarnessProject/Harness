using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace System.Events
{
    public interface INotify
    {
       void Subscribe<TX>(Action<TX> handler);
       Task Trigger<TX>(TX tEvent) where TX : IEvent;
    }

    public interface INotify<T> where T : IEvent
    {
        void Subscribe(Action<T> handler);
        Task Trigger(T tEvent);
    }
}
