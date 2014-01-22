#region ApacheLicense

// System.Portable.Base
// Copyright © 2014 Nick Daniels et all, All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License") with the following exception:
// 	Some source code is licensed under compatible licenses as required.
// 	See the attribution headers of the applicable source files for specific licensing 	terms.
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
using System.Threading.Tasks;

#endregion

namespace System.Portable.Runtime {
    public class NullDependencyProvider : IDependencyProvider {
        #region IDependencyProvider Members

        public object GetService(Type serviceType) {
            return default(object);
        }

        public object Get(Type serviceType) {
            return default(object);
        }

        public object Get(Type serviceType, string key) {
            return default(object);
        }

        public IEnumerable<object> GetAll(Type serviceType) {
            return default(IEnumerable<object>);
        }

        public TService Get<TService>() {
            return default(TService);
        }

        public TService Get<TService>(string key) {
            return default(TService);
        }

        public IEnumerable<TService> GetAll<TService>() {
            return default(IEnumerable<TService>);
        }

        public void Dispose() {}

        #endregion

        public T GetInstanceOf<T>(string type) {
            return default(T);
        }

        public bool GetImplementation<T>(Action<T> action) where T : IDependencyProvider {
            return false;
        }

        public Task<bool> GetImplementationAsync<T>(Action<T> action) where T : IDependencyProvider {
            return new Task<bool>(() => false);
        }
    }
}