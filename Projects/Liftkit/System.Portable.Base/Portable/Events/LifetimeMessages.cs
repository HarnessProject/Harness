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

using System.Contracts;
using System.Portable.App;

#endregion

namespace System.Portable.Events {
    public abstract class Event : IEvent {
        #region IEvent Members

        public object Sender { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime TimeStamp { get; set; }

        public IEvent Parent { get; set; }

        public ICancelToken Token { get; set; }

        #endregion
    }

    public abstract class ApplicationEvent<T> : Event {
        protected ApplicationEvent(object sender, IScope scope, IEvent parent = null) {
            Sender = sender;
            TimeStamp = DateTime.Now;
            Parent = parent;
            Scope = scope;
        }

        public IScope Scope { get; set; }
    }

    public class ApplicationStartEvent : ApplicationEvent<IScope> {
        public ApplicationStartEvent(object sender, IScope scope) : base(sender, scope) {
            Title = "Application Start";
            Description = "The Application has started";
        }
    }
}