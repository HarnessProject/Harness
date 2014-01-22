using System.Composition;
using System.Messaging;
using System.Portable.Runtime;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace System.Portable.Events
{
    public delegate void DelegateAction<T>(T action);

    public delegate void DelegateFilter<T>(T filter);

    public interface IEventManager {
        Task Trigger(IEvent evnt);
        Guid Handle<T>(DelegateAction<T> handler, DelegateFilter<T> filter = null) where T : IEvent;
        void RemoveHandler(Guid guid);
    }

    public class EventManager : IEventManager {
        public Task Trigger(IEvent evnt) {
            throw new NotImplementedException();
        }

        public Guid Handle<T>(DelegateAction<T> handler, DelegateFilter<T> filter = null) where T : IEvent {
            throw new NotImplementedException();
        }

        public void RemoveHandler(Guid guid) {
            throw new NotImplementedException();
        }
    }
}
