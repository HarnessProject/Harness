using System.Collections.Generic;
using System.Composition.Dependencies;
using System.Composition.Providers;
using Autofac;
using Autofac.Builder;

namespace System.Composition.Autofac {
    public class AutofacDependencyRegistration<T, TActivatorData, TRegistrationStyle> : IDependencyRegistration {
        private readonly IRegistrationBuilder<T, TActivatorData, TRegistrationStyle> _registration;
        private readonly ITypeProvider _typeProvider;
        private readonly Type _type;

        public AutofacDependencyRegistration(IRegistrationBuilder<T, TActivatorData, TRegistrationStyle> registration, ITypeProvider typeProvider, Type type) {
            _registration = registration;
            _typeProvider = typeProvider;
            _type = type;
            _registration.As<T>();
        }

        public IDependencyRegistration AsAny() {
            var types = new List<Type>();
            
            _typeProvider.GetAncestorsOf(_type).AddTo(types);
            _type.GetInterfaces().AddTo(types);

            types.Each(x => _registration.As(x));

            return this;
        }

        public IDependencyRegistration InjectProperties(bool preserveValues) {
            _registration.PropertiesAutowired(PropertyWiringOptions.PreserveSetValues);
            return this;
        }

        public IDependencyRegistration As<TY>() {
            _registration.As<TY>();
            return this;
        }

        public IDependencyRegistration As(Type type) {
            _registration.As(type);
            return this;
        }

        public IDependencyRegistration AsSingleton() {
            _registration.SingleInstance();
            return this;
        }

        public IDependencyRegistration AsTransient() {
            _registration.InstancePerDependency();
            return this;
        }

        public IDependencyRegistration AsSingleInScope() {
            _registration.InstancePerLifetimeScope();
            return this;
        }
    }
}