using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Practices.ServiceLocation;

namespace System.Dependency {
    /*
     * An implementation of the Common Service Locator Pattern, 
     * with support for obtaining the underlying implementation.
     */
    public interface IDependencyContainer : Microsoft.Practices.ServiceLocation.IServiceLocator, IDisposable
    {
        bool GetImplementation<T>(Action<T> action) where T : IServiceLocator;
        Task<bool> GetImplementationAsync<T>(Action<T> action) where T : IServiceLocator;
    }
}

// Portable IServiceLocator for recompiling components expecting the Microsoft ServiceLocator
namespace Microsoft.Practices.ServiceLocation {
    public interface IServiceLocator : IServiceProvider {
        object GetInstance(Type serviceType);
        object GetInstance(Type serviceType, string key);
        IEnumerable<object> GetAllInstances(Type serviceType);
        TService GetInstance<TService>();
        TService GetInstance<TService>(string key);
        IEnumerable<TService> GetAllInstances<TService>();
    }

    public delegate IServiceLocator ServiceLocatorProvider();

    public class ActivationException : Exception { }

    public static class ServiceLocator {
        private static IServiceLocator _locator;
        private static ServiceLocatorProvider _provider;
        public static void SetLocatorProvider(ServiceLocatorProvider newProvider) {
            _provider = newProvider;
        }

        public static IServiceLocator Current {
            get {
                if (_provider == null) return null;
                return _locator ?? (_locator = _provider());
            }
        }
    }

    public abstract class ServiceLocatorImplBase : IServiceLocator {
        public abstract object GetService(Type serviceType);
        public abstract object GetInstance(Type serviceType);
        public abstract object GetInstance(Type serviceType, string key);
        public abstract IEnumerable<object> GetAllInstances(Type serviceType);
        public abstract TService GetInstance<TService>();
        public abstract TService GetInstance<TService>(string key);
        public abstract IEnumerable<TService> GetAllInstances<TService>();
    }

}