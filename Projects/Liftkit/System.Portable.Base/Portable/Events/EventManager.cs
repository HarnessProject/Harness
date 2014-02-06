#region ApacheLicense

// System.Portable.Base
// Copyright © 2013 Nick Daniels et all, All Rights Reserved.
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

using System.Collections;
using System.Linq;
using System.Portable.Runtime;
using System.Portable.Runtime.Reflection;
using System.Threading.Tasks;

#endregion

namespace System.Portable.Events {
    public class EventManager : IEventManager {
        protected Pipeline Pipeline;

        public EventManager(IReflector invoker) {
            Pipeline = new Pipeline(invoker);
            App.TypeProvider.Types.Where(x => x.Is(typeof (IEventHandler<>))).Select(x => x.GetGenericArguments().First());
        }

        #region IEventManager Members

        public Task Trigger(IEvent evnt) {
            return Pipeline.Process(evnt); //THUNK 
        }

        public Guid Handle<T>(Action<T> handler, Filter<T> filter = null) where T : IEvent {
            return Pipeline.AddDelegate(handler, filter);
        }

        public void RemoveHandler(Guid guid) {
            Pipeline.RemoveDelegate(guid);
        }

        #endregion
    }
}