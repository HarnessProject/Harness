namespace System.Portable.Runtime {
    public interface IRegistrationProvider<T> {
        void Register(ITypeProvider typeProvider, T builder);
    }
}