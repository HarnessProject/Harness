using System;
using System.Collections;
using System.Collections.Generic;
using Autofac.Harness;
using System.Linq;
using Harness.Framework.Interfaces;
using Harness.Framework.Extensions;
using Harness.Framework;

namespace Autofac.Harness
{
    public class AutofacDependencyProvider : IDependencyProvider {
        public ILifetimeScope Container { get; set; }

        public AutofacDependencyProvider() : this(new AutofacContainerFactory().Create()) {}

        public AutofacDependencyProvider(ILifetimeScope container) {
            Container = container;
            Provider.State.$AutofacContainer = Container;
        }

        public void Dispose() {
            //Container.Dispose();
            Container.Disposer.Dispose();
            Container.Dispose();
        }

        public object GetService(Type serviceType) {
            if (Container.TryResolve(serviceType, out var r)) return r;
            return Provider.Domain.GetDefault(serviceType);
        }

        public IScope CreateScope() {
            return new Scope() { DependencyProvider = new AutofacDependencyProvider(Container.BeginLifetimeScope()) };
        }

        public object Get(Type serviceType) {
            return Container.Resolve(serviceType);
        }

        public object Get(Type serviceType, string key) {
            return Container.ResolveNamed(key, serviceType);
        }

        public IEnumerable<object> GetAll(Type serviceType) {
            return 
                Container
                .Resolve(
                    typeof(IEnumerable<>)
                    .MakeGenericType(serviceType)
                ).AsType<IEnumerable>()
                .Cast<object>();
        }

        private object AsType(IEnumerable enumerable)
        {
            throw new NotImplementedException();
        }

        public TService Get<TService>() {
            return Container.Resolve<TService>();
        }

        public TService Get<TService>(string key) {
            return Container.ResolveNamed<TService>(key);
        }

        public IEnumerable<TService> GetAll<TService>() {
            return Container.Resolve<IEnumerable<TService>>();
        }

        public void InjectProperties(object o)
        {
            Container.InjectProperties(o);
        }
    }
}
