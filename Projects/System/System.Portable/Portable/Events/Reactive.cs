using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;

namespace System.Portable.Events {
    public class Reactive<T> : IObservable<T>, IEventSource<T>, IDisposable {

        private T _value;
        

        public Reactive(T val) {
            _value = val;
        }

        protected void Next(T val) {
            OnNext.NotNull(o => o(val));
        }

        protected void Error(Exception ex) {
            OnError.NotNull(o => o(ex));
        }

        protected void Completed() {
            OnCompleted.NotNull(o => o());
        }

        public T Value {
            get {
                return _value;
            }
            set {
                SetValue(value);
            }
        }

        public void SetValue(T value) {
            this.Try(x => {
                _value = value;
                x.Next(value);
                return value;
            }).Catch<Exception>(
                (x, ex) => {
                    x.AsTask(y => y.Error(ex));
                    return default(T);
                }
            ).Act();
        }

        public Task SetValueAsync(T value) {
            return this.AsTask(x => x.SetValue(value));
        }

        public IDisposable Subscribe(IObserver<T> observer) {
            OnNext += observer.OnNext;
            OnError += observer.OnError;
            OnCompleted += observer.OnCompleted;
            return this;
        }

        public event Action<T> OnNext;
        public event Action<Exception> OnError;
        public event Action OnCompleted;
        public void Dispose() {
            Completed();
            if (typeof(T).Is<IDisposable>())
                Value.As<IDisposable>().Dispose();
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