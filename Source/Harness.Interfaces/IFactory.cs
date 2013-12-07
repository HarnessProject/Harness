using System.Threading.Tasks;

namespace Harness {
    public interface IFactory<T> {
        Task<T> CreateAsync();
        T Create();

    }
}