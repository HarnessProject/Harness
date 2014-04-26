using System;
using System.Reactive;
using System.Threading.Tasks;

namespace Harness.Framework.Reactive {
    

    public interface IReactive<T> : IObservable<T>, IEventSource<T>, IDisposable {
        T Value { get; set; }
        void SetValue(T value);
        Task SetValueAsync(T value);
        event Action<Exception> OnError;
        event Action OnCompleted;

    }
}