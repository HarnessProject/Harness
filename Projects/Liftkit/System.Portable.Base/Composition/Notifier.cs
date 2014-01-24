using System.Portable;
using System.Portable.Events;
using System.Portable.Runtime;
using System.Threading.Tasks;

namespace System.Composition
{
    public abstract class Notifier<T> : INotify<T> where T : IEvent {
        protected Notifier() {
            EventPipeline = App.Container.Get<Pipeline>();
        }

        protected Pipeline EventPipeline { get; set; }

        #region INotify<T> Members

        public async void Notify(T eEvent) {
            await EventPipeline.Process(eEvent);
        }

        public Task NotifyAsync(T eEvent) {
            return EventPipeline.Process(eEvent);
        }

        public void OnNotice(Action<T> action, Filter<T> filter) {
            EventPipeline.AddDelegate(action, filter);
        }

        #endregion
    }
}