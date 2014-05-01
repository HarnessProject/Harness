#region ApacheLicense
// From the Harness Project
// Harness.Framework.Net
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
using Harness.Framework.Extensions;

namespace Harness.Framework.Contracts {
    public interface IAssert
    {
        AssertionResult Assert(object val);
    }
    public class Assertion(
        Filter<object> filter,
        string invalidMessage = "",
        string validMessage = "") : IAssert
        {
        public Filter<object> Filter { get; set; }
        public string InvalidMessage { get; set; }
        public string ValidMessage { get; set; }

        public virtual AssertionResult Assert(object val) {
            var valid = Filter(val);
            return new AssertionResult {
                Valid = valid,
                Message = valid ? ValidMessage : InvalidMessage
            };
        }

        public static IAssert IsString() {
            return new Assertion( filter:  (o) => o.Is<string>(), invalidMessage:  "is not a string." );
        }
    }

    public class Assertion<T>(
        Filter<T> filter,  
        string invalidMessage = "",
        string validMessage = ""
        
        ) : IAssert
    {
        public Filter<T> Filter { get; } = filter;
        public string InvalidMessage { get; } = invalidMessage;
        public string ValidMessage { get; } = validMessage;
       
        AssertionResult IAssert.Assert(object val)
        {
            var valid = Filter(val.AsType<T>());
            return new AssertionResult
            {
                Valid = valid,
                Message = valid ? ValidMessage : InvalidMessage
            };
        }
    }
}