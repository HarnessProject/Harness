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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Harness.Framework.Extensions;

#endregion

namespace Harness.Framework.Collections {
    public static class EnumerationExtensions {
        public static IEnumerable<T> WhereIs<T>(this IEnumerable collection) {
            return collection.Cast<T>().WhereNotDefault();
        }

        public static IEnumerable<T> WhereNotDefault<T>(this IEnumerable<T> collection) {
            return collection.Where(x => x.NotDefault());
        }

        public static void Each<T>(this IEnumerable collection, Action<T> action) {
            collection.WhereIs<T>().Each(action);
        }

        public static void Each<T>(this IEnumerable collection, params Action<T>[] actions) {
            actions.Each(collection.Each);
        }

        public static void Each<T, TY>(this IEnumerable collection, TY state, params Action<T, TY>[] actions) {
            actions.Each(x => collection.Each(x, state));
        }

        public static void Each<T, TY>(this IEnumerable collection, Action<T, TY> action, TY state) {
            collection.Each<T>(i => action(i, state));
        }
    }
}