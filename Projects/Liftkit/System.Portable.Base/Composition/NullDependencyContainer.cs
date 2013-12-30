using System.Collections.Generic;
using System.Portable.Runtime;
using System.Threading.Tasks;

namespace System.Composition {
    public class NullDependencyContainer : IDependencyContainer
    {
        public object GetService(Type serviceType) {
            return default(object);
        }

        public object Obtain(Type serviceType) {
            return default(object);
        }

        public object Obtain(Type serviceType, string key) {
            return default(object);
        }

        public IEnumerable<object> ObtainAll(Type serviceType) {
            return default(IEnumerable<object>);
        }

        public TService Obtain<TService>() {
            return default(TService);
        }

        public TService Obtain<TService>(string key) {
            return default(TService);
        }

        public IEnumerable<TService> ObtainAll<TService>() {
            return default(IEnumerable<TService>);
        }

        public T GetInstanceOf<T>(string type) {
            return default(T);
        }

        public bool GetImplementation<T>(Action<T> action) where T : IDependencyContainer {
            return false;
        }

        public Task<bool> GetImplementationAsync<T>(Action<T> action) where T : IDependencyContainer {
            return new Task<bool>(() => false);
        }

        public void Dispose() {
            
        }
    }
}