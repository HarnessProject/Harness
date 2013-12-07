using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harness.Framework;
using TinyIoC;

namespace Harness
{
    

    public class TinyIoCServiceLocator : IServiceLocator {
        protected readonly TinyIoCContainer Container = new TinyIoCContainer();
        protected Func<Type, bool> Requirements<T>()
        {
            //You can override this to alter your Type requirements.
            return x =>
                Q.If<Type>(y => y.Is<T>())
                    .And(y => y.IsPublic)
                    .And(y => !y.IsAbstract)
                    .And(y => !y.IsInterface)(x);
        }

        public TinyIoCServiceLocator(IEnvironment environment, TinyIoCContainer parent = null) {
            if (parent != null) {
                Container = parent.GetChildContainer();
                return;
            }

            var ts = environment.GetTypes().ToArray();
            var ideps = ts.Where(x => Requirements<IDependency>()(x)).ToArray();
            
            var interfaces = ideps.SelectMany(x => x.GetInterfaces())
                .Where(
                Q.If<Type>(y => y != typeof(IDependency))
                    .And(y => y != typeof(ISingletonDependency))
                    .True ).Distinct();
            
            var implementationTable = new LookupTable<Type, Type>();
            interfaces.Each(x => implementationTable.Add(x, ideps.Where(y => y.Is(x)).ToArray()));
            implementationTable.Each(x => {
                var r = Container.RegisterMultiple(x.Key, x.ToArray());
                if (x.Key.Is<ISingletonDependency>()) r.AsSingleton();
                else r.AsMultiInstance();
            });

            

        }

       
        public object GetService(Type serviceType) {
            return serviceType.Is<ISingletonDependency>() ? 
                (Container.CanResolve(serviceType) ? Container.Resolve(serviceType) : null) : 
                null;
        }

        public object GetInstance(Type serviceType) {
            return Container.CanResolve(serviceType) ? Container.Resolve(serviceType) : null;
        }

        public object GetInstance(Type serviceType, string key) {
            return Container.CanResolve(serviceType, key, ResolveOptions.Default) ? Container.Resolve(serviceType, key) : null;
        }

        public IEnumerable<object> GetAllInstances(Type serviceType) {
            return Container.CanResolve(serviceType) ? Container.ResolveAll(serviceType) : null;
        }

        public TService GetInstance<TService>() {
            var t = typeof (TService);
            return GetService(t).As<TService>();
        }

        public TService GetInstance<TService>(string key) {
            var t = typeof(TService);
            return GetInstance(t, key).As<TService>();
        }

        public IEnumerable<TService> GetAllInstances<TService>() {
            var t = typeof(TService);
            return GetAllInstances(t).Cast<TService>();
        }

        public void Dispose() {
            Container.Dispose();
        }

        public bool GetImplementation<T>(Action<T> action) where T : IServiceLocator {
            return
                this.Try(x => {
                    if (typeof(TinyIoCServiceLocator).Is<T>())
                    {
                        action(x.As<T>());
                        return true;
                    }
                    return false;
                }).Catch<Exception>((x, ex) => false)
                .Invoke();
        }

        public Task<bool> GetImplementationAsync<T>(Action<T> action) where T : IServiceLocator {
            return Task<bool>.Factory.StartNew(a => GetImplementation(a as Action<T>), action);
        }
    }
}
