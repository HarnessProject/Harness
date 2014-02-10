namespace System.Portable.Events {
    public class ActionObserver<T>  : IObserver<T> {
        private readonly Action<T> _action;

        public ActionObserver(Action<T> action) {
            _action = action;
        }

        public void OnNext(T value) {
            _action(value);
        }

        public void OnError(Exception error) {}
        public void OnCompleted() {}
    }
}