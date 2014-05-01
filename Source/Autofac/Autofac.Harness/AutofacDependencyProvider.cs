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
