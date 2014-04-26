using System.Composition.Dependencies;
using System.Composition.Providers;
using System.Linq;
using System.Portable;
using System.Reflection;
using Autofac;
using Autofac.Builder;

namespace System.Composition.Autofac {
    public class AutofacDependencyRegistrar : IDependencyRegistrar {
        public readonly ContainerBuilder Builder;
        private readonly ITypeProvider _typeProvider;

        public AutofacDependencyRegistrar(ContainerBuilder builder, ITypeProvider typeProvider) {
            Builder = builder;
            _typeProvider = typeProvider;
        }

        public IDependencyRegistration Register(Type type) {
            return new AutofacDependencyRegistration<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>(Builder.RegisterType(type), _typeProvider, type);
            
        }

        public IDependencyRegistration Register<T>() {
            return new AutofacDependencyRegistration<T, ConcreteReflectionActivatorData, SingleRegistrationStyle>(Builder.RegisterType<T>(), _typeProvider, typeof(T));
        }

        public IDependencyRegistration FactoryFor<T>(Func<T> creator) {
            return new AutofacDependencyRegistration<T, SimpleActivatorData, SingleRegistrationStyle>(Builder.Register<T>(c => creator()), _typeProvider, typeof(T));
        }
    }
}