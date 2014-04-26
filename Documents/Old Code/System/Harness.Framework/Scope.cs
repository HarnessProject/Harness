using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Harness.Framework.Interfaces;

namespace Harness.Framework
{
    public class Scope : IScope
    {
        public IDependencyProvider DependencyProvider { get; set; }
        public IDictionary<string,object> State { get; set; } = new Dictionary<string,object>();

        public object Get(Type serviceType)
        {
            return DependencyProvider.Get(serviceType);
        }

        public object Get(Type serviceType, string key)
        {
            return DependencyProvider.Get(serviceType, key);
        }

        public T Get<T>()
        {
            return DependencyProvider.Get<T>();
        }

        public T Get<T>(string key)
        {
            return DependencyProvider.Get<T>(key);
        }

        public IEnumerable<object> GetAll(Type serviceType)
        {
            return DependencyProvider.GetAll(serviceType);
        }

        public IEnumerable<T> GetAll<T>()
        {
            return DependencyProvider.GetAll<T>();
        }
    }
}
