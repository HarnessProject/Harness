#region ApacheLicense

// From the Harness Project
// System.Portable
// Copyright © 2014 Nick Daniels et all, All Rights Reserved.
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

#region

using System.Collections.Generic;
using System.Composition.Providers;
using Newtonsoft.Json.Linq;

#endregion

namespace System.Composition {
    public class Scope : IScope {
        public Scope(IDependencyProvider container) {
            DependencyProvider = container;
            State = new JObject();
        }

        #region IScope Members

        public IDependencyProvider DependencyProvider { get; set; }

        public dynamic State { get; private set; }

        public object Get(Type serviceType) {
            return DependencyProvider.Get(serviceType);
        }

        public object Get(Type serviceType, string key) {
            return DependencyProvider.Get(serviceType, key);
        }

        public IEnumerable<object> GetAll(Type serviceType) {
            return DependencyProvider.GetAll(serviceType);
        }

        public T Get<T>() {
            return DependencyProvider.Get<T>();
        }

        public T Get<T>(string key) {
            return DependencyProvider.Get<T>(key);
        }

        public IEnumerable<T> GetAll<T>() {
            return DependencyProvider.GetAll<T>();
        }

        #endregion

        public void Dispose() {
            DependencyProvider.Dispose();
        }

        public object GetService(Type serviceType) {
            return DependencyProvider.GetService(serviceType);
        }
    }
}