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

namespace System {
    public static class Filter {
        public static bool If<T>(this T target, Filter<T> condition, Action<T> thenAction = null, Action<T> elseAction = null) where T : class {
            bool result = condition(target);
            if (!result) return false;
            if (thenAction != null) thenAction(target);
            else if (elseAction != null) elseAction(target);
            return true;
        }

        public static Filter<T> If<T>(Filter<T> func) {
            return func;
        }

        public static Filter<T> And<T>(this Filter<T> func, Filter<T> nFunc) {
            return x => func(x) && nFunc(x);
        }

        public static Filter<T> Or<T>(this Filter<T> func, Filter<T> nFunc) {
            return x => func(x) || nFunc(x);
        }

        public static bool Result<T>(this Filter<T> func, T target = default(T)) {
            return func.Try(x => x(target)).Catch<Exception>((y, ex) => false).Act();
        }

        public static bool True<T>(this Filter<T> func, T target = default(T)) {
            return func.Result(target);
        }

        public static Filter<T> Then<T>(this Filter<T> action, Action<T> then) {
            return x => {
                bool r = If(action).True(x);
                if (r) then(x);

                return r;
            };
        }

        public static Filter<T> Then<T>(this Filter<T> action, params Action<T>[] actions) {
            return x => {
                bool r = If(action).True(x);
                if (r) actions.Each(y => y(x));

                return r;
            };
        }

        public static Filter<T> Else<T>(this Filter<T> func, Action<T> action) {
            return x => {
                bool r = If(func).True(x);

                if (!r) action(x);

                return r;
            };
        }

        public static bool False<T>(this Filter<T> func, T target) {
            return func.Result(target) == false;
        }
    }
}