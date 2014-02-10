using System.Reactive.Disposables;

namespace System.Portable.Events {
    public abstract class RelayObserver<T,TY> : IObserver<T>, IObservable<TY> {

        public event Action<TY> Next;
        public event Action<Exception> Error;
        public event Action Complete;

        public abstract TY Process(T t);

        public void OnNext(T value) {
            Next.NotNull(n => n(Process(value)));
        }

        public void OnError(Exception error) {
            Error.NotNull(e => e(error));
        }

        public void OnCompleted() {
            Complete.NotDefault(c => c());
        }

        public IDisposable Subscribe(IObserver<TY> observer) {
            Next += observer.OnNext;
            Error += observer.OnError;
            Complete += observer.OnCompleted;
            return Disposable.Empty;
        }
    }
}