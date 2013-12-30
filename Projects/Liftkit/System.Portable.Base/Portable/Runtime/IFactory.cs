using System.Threading.Tasks;

namespace System.Portable.Runtime {
    public interface IFactory<T> {
        Task<T> CreateAsync();
        T Create();

    }
}