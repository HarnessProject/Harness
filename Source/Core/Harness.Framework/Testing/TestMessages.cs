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
using System.Text;
using Harness.Framework.Extensions;

namespace Harness.Framework.Testing
{
    public static class TestMessages
    {
        public static string NotRegistered<T>()
        {
            return NotRegistered(typeof(T));
        }
        public static string NotRegistered(Type type)
        {
            return "{0} is not registered".WithParams(type.Name);
        }

        public static string IsNullOrDefault<T>(string variable)
        {
            return IsNullOrDefault(variable, typeof(T));
        }

        public static string IsNullOrDefault(string variable, Type type)
        {
            return "{0} is null or default({1})".WithParams(variable, type.Name);
        }

        public static string Returned<T>(string operation, T result, T expected)
        {
            return "{0} returned {1}, not {2}".WithParams(operation, result, expected);
        }

        public static string NotTrue(string operation)
        {
            return Returned(operation, false, true);
        }

        public static string NotFalse(string operation)
        {
            return Returned(operation, true, false);
        }

        public static string NotEqual(string val1, string val2)
        {
            return "{0} does not equal {1}".WithParams(val1, val2);
        }
    }
}
