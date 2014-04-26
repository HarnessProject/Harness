using System.Collections;
using System.Collections.Generic;
using System.Composition.Dependencies;
using System.Composition.Providers;
using System.Composition.Providers.Null;
using System.Linq;
using System.Portable;
using System.Threading.Tasks;
using Autofac;

namespace System.Composition.Autofac
{
    [SuppressDependency(typeof(NullDependencyProvider))]
    public class AutofacDependencyProvider : IDependencyProvider {
        public ILifetimeScope Container { get; set; }

        public AutofacDependencyProvider() {
            Container = new AutofacContainerFactory().Create(new { TypeProvider = Provider.Types, Instance = this });
        }

        public AutofacDependencyProvider(ILifetimeScope container) {
            Container = container;
        }

        public void Dispose() {
            //Container.Dispose();
            Container.Disposer.Dispose();
            Container.Dispose();
        }

        public object GetService(Type serviceType) {
            return Container.Resolve(serviceType);
        }

        public IDependencyProvider CreateScope() {
            return new AutofacDependencyProvider( Container.BeginLifetimeScope() );
        }

        public object Get(Type serviceType) {
            return Container.Resolve(serviceType);
        }

        public object Get(Type serviceType, string key) {
            return Container.ResolveNamed(key, serviceType);
        }

        public IEnumerable<object> GetAll(Type serviceType) {
            return (
                (IEnumerable)Container
                .Resolve(
                    typeof(IEnumerable<>)
                    .MakeGenericType(serviceType))
                ).Cast<Object>();
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

        
    }
}
