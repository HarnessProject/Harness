using System.Contracts;
using System.Portable;
using System.Portable.Events;
using System.Portable.Runtime;
using System.Reactive.Subjects;
using System.Threading;
using System.Threading.Tasks;

namespace System.Composition
{
    public abstract class Notifier<T> : INotify<T> where T : IEvent {

        protected ISubject<T> EventSubject;

        protected Notifier() {
            EventSubject = new Subject<T>();
        } 
        
        public IDisposable Subscribe(IObserver<T> observer) {
            return EventSubject.Subscribe(observer);
        }

        public void Notify(T eEvent) {
            EventSubject.OnNext(eEvent);
        }

        public Task NotifyAsync(T eEvent) {
            return this.AsTask(x => x.Notify(eEvent));
        }
    }

    public interface IDisposableToken : IDisposable {
        event Action OnDisposing;
        bool Disposed { get; }
    }
}