using System.Portable;
using System.Portable.Events;
using System.Portable.Runtime;
using System.Threading.Tasks;

namespace System.Composition
{
    public abstract class Notifier<T> : INotify<T> where T :IEvent {
        protected Notifier() {
            Pipeline = App.Container.Get<Pipeline>();
        }

        protected Pipeline Pipeline { get; set; }

        #region INotify<T> Members

        public async void Notify(T eEvent) { // Fire and forget notify.
            await Pipeline.Process(eEvent);
        }

        public Task NotifyAsync(T eEvent) {
            return Pipeline.Process(eEvent);
        }

        public void OnNotice(Action<T> action, Filter<T> filter)  {
            Pipeline.AddDelegate(action, filter);
        }

        #endregion
    }
}