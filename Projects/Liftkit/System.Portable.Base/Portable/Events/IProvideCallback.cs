namespace System.Portable.Events {
    public interface IProvideCallback<in T> {
        void Callback(T message);
    }
}