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
using Harness.Framework.Collections;
using Harness.Framework.Extensions;

#endregion

namespace Harness.Framework {
    public static class Filter {
        public static bool If<T>(this T target, Filter<T> condition, Action<T> thenAction = null, Action<T> elseAction = null, Action<T> finallyAction = null) where T : class {
            bool result = condition(target);
            if (result && thenAction != null) thenAction(target);
            else if (!result && elseAction != null) elseAction(target);
            if (finallyAction != null) finallyAction(target);
            return result;
        }

        public static TY If<T,TY>(this T target, Filter<T> condition, Func<T,TY> thenFunc = null, Func<T,TY> elseFunc = null, Action<TY> finallyAction = null ) where T : class
        {
            bool result = condition(target);
            TY r = default(TY);
            if (result && thenFunc != null) r = thenFunc(target);
            else if (!result && elseFunc != null) r = elseFunc(target);
            if (finallyAction != null) finallyAction(r);
            return r;
        }

        public static Filter<T> If<T>(Filter<T> func) {
            return func;
        }

        public static Filter<T> IfNot<T>(Filter<T> func)
        {
            return x => !func(x);
        }

        public static Filter<T> And<T>(this Filter<T> func, Filter<T> nFunc) {
            return x => func(x) && nFunc(x);
        }

        public static Filter<T> AndNot<T>(this Filter<T> func, Filter<T> nFunc)
        {
            return x => func(x) && !nFunc(x);
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