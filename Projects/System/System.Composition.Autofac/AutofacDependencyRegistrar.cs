using System.Composition.Dependencies;
using System.Composition.Providers;
using System.Linq;
using System.Portable;
using System.Reflection;
using Autofac;
using Autofac.Builder;

namespace System.Composition.Autofac {
    public class AutofacDependencyRegistrar : IDependencyRegistrar {
        private readonly ContainerBuilder _builder;
        private readonly ITypeProvider _typeProvider;

        public AutofacDependencyRegistrar(ContainerBuilder builder, ITypeProvider typeProvider) {
            _builder = builder;
            _typeProvider = typeProvider;
        }

        public IDependencyRegistration Register(Type type) {
            return new AutofacDependencyRegistration<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>(_builder.RegisterType(type), _typeProvider, type);
            
        }

        public IDependencyRegistration Register<T>() {
            return new AutofacDependencyRegistration<T, ConcreteReflectionActivatorData, SingleRegistrationStyle>(_builder.RegisterType<T>(), _typeProvider, typeof(T));
        }

        public IDependencyRegistration FactoryFor<T>(Func<T> creator) {
            return new AutofacDependencyRegistration<T, SimpleActivatorData, SingleRegistrationStyle>(_builder.Register<T>(c => creator()), _typeProvider, typeof(T));
        }
    }
}