#region ApacheLicense
// From the Harness Project
// Harness.Framework
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
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;

namespace Harness.Framework.Dependencies
{
    public class RegistrationContext : IRegistrationContext {
        

        private readonly Dictionary<Type, List<Action<IDependencyRegistration>>>
            _handlers = new Dictionary<Type, List<Action<IDependencyRegistration>>>();

        #region IRegistrationContext Members

        public void RegistrationHandlerForType<T>(Action<IDependencyRegistration> registration) {
            var t = typeof (T);
            if (!_handlers.ContainsKey(t)) 
                _handlers.Add(t, new List<Action<IDependencyRegistration>>());

            _handlers[t].Add(registration);
        }

        #endregion

        public IEnumerable<Action<IDependencyRegistration>> HandlersFor(Type t) {
            return from key in _handlers.Keys where key.Is(t) from h in _handlers[key] select h;
        }
    }
}