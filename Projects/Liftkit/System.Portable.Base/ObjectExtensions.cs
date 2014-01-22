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

using System.Collections.Generic;
using System.Portable;

#endregion

namespace System {
    public static class ObjectExtensions {
        public static T Action<T>(this T obj, params Action<T>[] actions) {
            actions.Each(x => x(obj));
            return obj;
        }

        public static TY Func<T, TY>(this T obj, Func<T, TY> func) {
            return func(obj);
        }

        public static bool NotNull<T>(this T o) where T : class {
            return o != null;
        }

        public static T NotNull<T>(this T o, Action<T> action) where T : class {
            var b = o != null;
            if (b) action(o);
            return o;
        }

        public static bool NotDefault<T>(this T o) {
            return App.TypeProvider.GetDefault<T>().As<object>() == o.As<object>();
        }

        public static bool IsNull<T>(this T o) where T : class {
            return o == null;
        }

        public static T IsNull<T>(this T o, Func<T> initializer) {
            return initializer();
        }

        // ONLY TO BE USED FOR TESTING PURPOSES
        // Makes a one liner end in whatever value you want.
        // It's lazy, and shouldn't be used in production code.
        public static T Return<T>(this Object o, T val = default (T)) {
            return val;
        }
    }
}