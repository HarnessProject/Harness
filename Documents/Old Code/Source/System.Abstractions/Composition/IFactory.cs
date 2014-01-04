using System.Threading.Tasks;

namespace System.Composition {
    public interface IFactory<T> {
        Task<T> CreateAsync();
        T Create();

    }
}