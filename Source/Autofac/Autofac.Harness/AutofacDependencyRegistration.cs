using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Builder;
using Harness.Framework.Collections;
using Harness.Framework.Interfaces;

namespace Autofac.Harness
{
    public class AutofacDependencyRegistration<T, TActivatorData, TRegistrationStyle> : IDependencyRegistration {
        private readonly IRegistrationBuilder<T, TActivatorData, TRegistrationStyle> _registration;
        private readonly IDomainProvider _typeProvider;
        private readonly Type _type;

        public AutofacDependencyRegistration(IRegistrationBuilder<T, TActivatorData, TRegistrationStyle> registration, IDomainProvider typeProvider, Type type) {
            _registration = registration;
            _typeProvider = typeProvider;
            _type = type;
        }

        public IDependencyRegistration AsAny() {
            
            return 
                AsSelf()
                .AsAncestors()
                .AsImplemented();
        }

        public IDependencyRegistration InjectProperties(bool preserveValues) {
            _registration.PropertiesAutowired(preserveValues ? PropertyWiringOptions.PreserveSetValues : PropertyWiringOptions.None);
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

        public IDependencyRegistration AsSelf()
        {
            return As(_type);
        }

        public IDependencyRegistration AsImplemented()
        {
            return RegisterAsEach(_type.GetInterfaces());
        }

        public IDependencyRegistration AsAncestors()
        {
            return RegisterAsEach(_typeProvider.GetAncestorsOf(_type));
        }

        public IDependencyRegistration RegisterAsEach(params Type[] types)
        {
            types.Each(x => As(x));
            return this;
        }

        public IDependencyRegistration RegisterAsEach(IEnumerable<Type> types)
        {
            return RegisterAsEach(types.ToArray());
        }
    }
}