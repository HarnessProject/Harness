using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;

namespace System.Composition {

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
            return new Token();
        }
    }

    public class Reactive<T> : IObservable<T>, IEventSource<T>, IDisposable {

        private T _value;
        private readonly List<Token> _subscriberTokens = new List<Token>();

        public Reactive(T val) {
            _value = val;
        }

        protected void Next(T val) {
            if (OnNext.NotNull()) OnNext(val);
        }

        protected void Error(Exception ex) {
            if (OnError.NotNull()) OnError(ex);
        }

        protected void Completed() {
            if (OnCompleted.NotNull()) OnCompleted();
        }

        public T Value {
            get {
                return _value;
            }
            set {
                this.Try(x => {
                    _value = value;
                    Next(value);
                    return value;
                })
                    .Catch<Exception>((x, ex) => {
                        Error(ex);
                        return default(T);
                    }).Act();
            }
        }

        public IDisposable Subscribe(IObserver<T> observer) {
            OnNext += observer.OnNext;
            OnError += observer.OnError;
            OnCompleted += observer.OnCompleted;
            var t = new Token();
            _subscriberTokens.Add(t);
            return t;
        }

        public event Action<T> OnNext;
        public event Action<Exception> OnError;
        public event Action OnCompleted;
        public void Dispose() {
            Completed();
            _subscriberTokens.Each(x => x.Dispose());
        }

        public static implicit operator T(Reactive<T> val) {
            return val.Value;
        }

        public static implicit operator Reactive<T>(T val) {
            return new Reactive<T>(val);
        }

        public override string ToString() {
            return Value.ToString();
        }
    }
}