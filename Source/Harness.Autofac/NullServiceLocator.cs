using System;
using System.Collections.Generic;

namespace Harness.Autofac {
    public class NullServiceLocator : Harness.IServiceLocator
    {
        public object GetService(Type serviceType) {
            return default(object);
        }

        public object GetInstance(Type serviceType) {
            return default(object);
        }

        public object GetInstance(Type serviceType, string key) {
            return default(object);
        }

        public IEnumerable<object> GetAllInstances(Type serviceType) {
            return default(IEnumerable<object>);
        }

        public TService GetInstance<TService>() {
            return default(TService);
        }

        public TService GetInstance<TService>(string key) {
            return default(TService);
        }

        public IEnumerable<TService> GetAllInstances<TService>() {
            return default(IEnumerable<TService>);
        }

        public bool GetImplementation<T>(Action<T> action) where T : Harness.IServiceLocator {
            return false;
        }
    }
}