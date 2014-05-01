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
#region

using System.Collections.Generic;

using System.Threading.Tasks;
using Harness.Framework;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;
using Harness.Framework.Reactive;
using Harness.Framework.Tasks;

#endregion

namespace System.Composition.Events {
    public class EventManager : Relay<IEvent, IEvent>, IEventManager {
        protected IEnumerable<Type> Handlers; 
        public EventManager() {
            
        }   
    
        #region IEventManager Members

        public IDisposable Handle<T>(Action<T> next, Action<Exception> error = null, Action complete = null) where T : IEvent {
            if (error.NotNull() && complete.NotNull())
                return this.WhereIs<T>().Subscribe(next, error, complete);
            return error.NotNull() ? this.WhereIs<T>().Subscribe(next, error) : this.WhereIs<T>().Subscribe(next);
        }

        public async void Trigger<T>(T evnt) where T : IEvent {
            await this.AsTask(x => x.OnNext(evnt));
            await Provider.GetAll<IEventHandler<T>>().EachAsync(h => h.Handle(evnt));
        }

        #endregion

        protected override IEvent Process(IEvent t) {
            return t;
        }
    }
}