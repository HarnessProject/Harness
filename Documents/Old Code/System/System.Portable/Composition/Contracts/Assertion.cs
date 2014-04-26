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

#endregion

namespace System.Contracts {
    public class Assertion<T, TY> : Assertion {
        public Assertion(Func<T, TY, bool> assertion, string invalidMessage) : base(assertion, invalidMessage) {}
    }

    public class Assertion<T> : Assertion {
        public Assertion(Filter<T> assertion, string invalidMessage) : base(assertion, invalidMessage) {}
    }

    public abstract class Assertion {
        protected Assertion(Delegate assertion, string invalidMessage, IList<object> args = null) {
            Predicate = assertion;
            InvalidMessage = invalidMessage;
            Arguments = args ?? new List<object>();
        }

        public IList<object> Arguments { get; protected set; }
        public string InvalidMessage { get; protected set; }

        public Delegate Predicate { get; protected set; }
    }
}