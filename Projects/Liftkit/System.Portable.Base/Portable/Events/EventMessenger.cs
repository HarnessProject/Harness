using System.Composition;
using System.Events;
using System.Messaging;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Portable.Events
{

    public interface IEventManager {
        Task Trigger(IEvent evnt);
        Guid Handle<T>(DelegateAction<T> handler, DelegateFilter<T> filter = null) where T : IEvent;
        void RemoveHandler(Guid guid);
    }

    public class EventManager : IEventManager {
        
    }
}
