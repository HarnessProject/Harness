using System.Threading.Tasks;

namespace Harness {
    public interface IDispatch {
        Task SendAsync<T>(T message);
        void Receive<T>(params MessageHandler<T>[] handlers) where T : class;
    }
}