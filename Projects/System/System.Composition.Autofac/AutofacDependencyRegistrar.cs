using System.Composition.Dependencies;
using System.Composition.Providers;
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
            return new AutofacDependencyRegistration<object, ConcreteReflectionActivatorData, SingleRegistrationStyle>(_builder.RegisterType(type), _typeProvider);
        }

        public IDependencyRegistration Register<T>(Type type) {
            return new AutofacDependencyRegistration<T, ConcreteReflectionActivatorData, SingleRegistrationStyle>(_builder.RegisterType<T>(), _typeProvider);
        }

        public IDependencyRegistration FactoryFor<T>(Func<T> creator) {
            return new AutofacDependencyRegistration<T, SimpleActivatorData, SingleRegistrationStyle>(_builder.Register<T>(c => creator()), _typeProvider);
        }
    }
}