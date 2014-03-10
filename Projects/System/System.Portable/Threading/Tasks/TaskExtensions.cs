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
using System.Linq;

#endregion

namespace System.Threading.Tasks {
    public static class TaskExtensions {
        public static async Task EachAsync<T>(this IEnumerable<T> collection, Action<T> action) {
            await collection.AsTask(
                c => Task.WaitAll(
                    c.Select(
                        x => x.AsTask(action)
                        ).ToArray()
                    )
                );
        }

        public static async Task EachAsync<T>(this Task<IEnumerable<T>> collection, Action<T> action) {
            await (await collection).EachAsync(action);
        }

        public static async Task<IQueryable<TY>> SelectAsync<T, TY>(this Task<IEnumerable<T>> collection, Func<T, TY> action) {
            return await (await collection).AsTask(c => c.Select(action).AsQueryable());
        }

        public static Task EachAsync<T, TY>(this IEnumerable<T> collection, Action<T, TY> action, TY context) {
            return collection.AsTask(c =>
                                         Task.WaitAll(
                                             c.Select(x =>
                                                          x.AsTask(y =>
                                                                       action(y, context)
                                                          )
                                                 ).ToArray()
                                         )
                );
        }

        public static async Task EachAsync<T, TY>(this Task<IEnumerable<T>> collection, Action<T, TY> action, TY context) {
            await EachAsync(await collection, action, context);
        }

        public static async Task ProcessAsync<T>(this IEnumerable<T> collection, params Action<T>[] actions) {
            await collection.EachAsync(async x => await actions.EachAsync(y => y(x)));
        }

        public static Task<T> AsTask<T>(this T t) {
            return Task.Factory.StartNew(() => t);
        }

        public static Task<TY> AsTask<T, TY>(this T t, Func<T, TY> func) {
            return Task.Factory.StartNew(x => func(x.As<T>()), t);
        }

        public static Task AsTask<T>(this T t, Action<T> action) {
            return Task.Factory.StartNew(x => action(x.As<T>()), t);
        }

        public static async Task<TY> AsTask<T, TY>(this T t, Func<T, Task<TY>> func) {
            return await func(t);
        }

        public static void Await(this Task task) {
            task.Wait();
        }

        public static T AwaitResult<T>(this Task<T> task) {
            task.Await();
            return task.Result;
        }
    }
}