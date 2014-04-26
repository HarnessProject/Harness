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
using System.Linq.Expressions;

#endregion

namespace System.Composition.Data {
    public interface IRepository<T> : IDisposable where T : class, new() {
        IEnumerable<T> GetByKey<TKey>(params TKey[] keys);
        IList<T> Select(Expression<Filter<T>> filter);
        void Add(params T[] items);
        int Update(params T[] items);
        int Delete(params T[] items);
        void Abort();
    }
}