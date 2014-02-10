using System.Portable.Reflection;

namespace System.Portable.Runtime {
    public interface IRegistrationProvider<in T> {
        void Register(ITypeProvider typeProvider, T builder);
    }
}