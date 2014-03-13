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
using System.Portable;
using System.Threading.Tasks;

#endregion

namespace System {
    public delegate Task<TY> AsyncFunc<in T, TY>(T o);

    public delegate Task AsyncAction<in T>(T t);

    public static class ArrayExtensions {
        public static int LastIndex(this Array array) {
            return array.Length - 1;
        }
    }

    public static class ObjectExtensions {
        public static T Action<T>(this T obj, params Action<T>[] actions) {
            actions.Each(x => x(obj));
            return obj;
        }

        public static TY Func<T, TY>(this T obj, Func<T, TY> func) {
            return func(obj);
        }

        public static T True<T>(this T t, Filter<T> filter, Action<T> action) {
            if (filter(t)) action(t);
            return t;
        }

        public static T False<T>(this T t, Filter<T> filter, Action<T> action) {
            if (!filter(t)) action(t);
            return t;
        }

        public static bool NotNull<T>(this T o) where T : class {
            return o != null;
        }

        public static T NotNull<T>(this T o, Action<T> action) where T : class {
            var n = NotNull(o);
            if (n) action(o);
            return o;
        }

        public static async Task<T> NotNullAsync<T>(this T o, Action<T> asyncAction) where T : class {
            var n = NotNull(o);
            if (n) await o.AsTask(asyncAction);
            return o;
        }

        public static bool NotDefault<T>(this T o) {
            return !IsDefault(o);
        }

        public static bool IsNull<T>(this T o) where T : class {
            return o == null;
        }

        public static T IsNull<T>(this T o, Func<T> initializer) where T : class {
            return IsNull(o) ? initializer() : o;
        }

        public static async Task<T> IsNullAsync<T>(this T o, Func<T> asyncInit) where T : class {
            return IsNull(o) ? await o.AsTask(x => asyncInit()) : o;
        }

        public static async Task<T> IsNullAsync<T>(this T o, Func<Task<T>> asyncInit) where T : class {
            return IsNull(o) ? await asyncInit() : o;
        }

        public static bool IsDefault<T>(this T t) {

            var d = Provider.Types.GetDefault<T>();
            return EqualityComparer<T>.Default.Equals(d, t);

        }

        public static T IsDefault<T>(this T t, Func<T> initializer) {
            return IsDefault(t) ? initializer() : t;
        }

        public static T NotDefault<T>(this T o, Action<T> action) {
            if (NotDefault(o))
                action(o);
            return o;
        }
    }
}