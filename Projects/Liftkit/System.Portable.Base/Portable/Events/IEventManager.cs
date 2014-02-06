using System.Portable.Runtime;
using System.Threading.Tasks;

namespace System.Portable.Events {
    public interface IEventManager : IDependency {
        Task Trigger(IEvent evnt);
        Guid Handle<T>(Action<T> handler, Filter<T> filter = null) where T :  IEvent;
        void RemoveHandler(Guid guid);
    }
}