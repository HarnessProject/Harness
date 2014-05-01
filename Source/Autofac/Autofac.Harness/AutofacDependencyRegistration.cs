#region ApacheLicense
// From the Harness Project
// Autofac.Harness
// Copyright © 2014 Nick Daniels, All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License") with the following exception:
// 	Some source code is licensed under compatible licenses as required.
// 	See the attribution headers of the applicable source files for specific licensing terms.
// 
// You may not use this file except in compliance with its License(s).
// 
// You may obtain a copy of the Apache License, Version 2.0 at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 
#endregion
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