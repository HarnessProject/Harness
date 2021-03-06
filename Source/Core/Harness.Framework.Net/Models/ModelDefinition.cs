﻿#region ApacheLicense
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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Harness.Framework.Models
{
    public delegate void ModelDefinition<T, TKey>(
           Expression<Func<T, TKey>> key = null,  
           Func<T> newModel = null,
           Func<TKey> defaultKey = null,
           Func<TKey, TKey, bool> keysEqual = null,
           Func<IEnumerable<T>> emptyCollection = null,
           Func<IEnumerable<T>> all = null,
           Func<IQueryable<T>> asQueryable = null);
}
