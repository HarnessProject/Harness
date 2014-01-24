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

using System.Threading.Tasks;

#endregion

namespace System.Collections.Generic {
    public static class EnumerableExtensions {
        public static async void Each<T>(this IEnumerable<T> collection, Action<T> action) {
            await collection.EachAsync(action);
        }

        public static void Each<T>(this IEnumerable<T> collection, params Action<T>[] actions) {
            actions.Each(collection.Each);
        }

        public static void Each<T, TY>(this IEnumerable<T> collection, TY state, params Action<T, TY>[] actions) {
            actions.Each(x => collection.Each(x, state));
        }

        public static void Each<T, TY>(this IEnumerable<T> collection, Action<T, TY> action, TY state) {
            collection.Each(x => action(x, state));
        }

        public static TY AddTo<T, TY>(this IEnumerable<T> collection, TY list) where TY : IList<T> {
            collection.Each(list.Add);
            return list;
        }

        public static IList<TY> AddTo<T, TY>(this IEnumerable<T> collection, IList<TY> list, Func<T, TY> transform) 
        {
            collection.Each(x =>
            {
                var i = transform(x);
                list.Add(i);
            });
            return list;
        }
    }
}