#region ApacheLicense
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
using System.Reflection;
using Harness.Framework.Collections;
using Harness.Framework.Extensions;

namespace Harness.Framework.Models
{
   
    

    

    public class Model<T, TKey> : IModel
    {
        public static MemberInfo Key { get; private set; }
        public static Func<IEnumerable<T>> All { get; private set; } = EmptyArray;
        public static Func<IQueryable<T>> AsQueryable { get; private set; } = EmptyArray().AsQueryable;
        public static Func<TKey> DefaultKey { get; private set; } = () => default(TKey);
        public static Func<IEnumerable<T>> EmptyCollection { get; private set; } = EmptyArray;
        public static Func<TKey, TKey, bool> KeysEqual { get; private set; } = (k1, k2) => EqualityComparer<TKey>.Default.Equals(k1, k2);

        public static Func<T> New { get; private set; } = () => {
            var t = typeof(T);
            var result = t.CreateInstance().AsType<T>();
            Provider.Reflector.SetPropertyValue(result, Key.Name, DefaultKey());
            Provider.Dependencies.InjectProperties(result);
            return result;
        };

        public void Initialize()
        {
            Define(DefineModel);
        }

        public virtual void Define(ModelDefinition<T,TKey> defineModel) { }
        public static void DefineModel(
            Expression<Func<T, TKey>> key,
            Func<T> newModel = null,
            Func<TKey> defaultKey = null,
            Func<TKey, TKey, bool> keysEqual = null,
            Func<IEnumerable<T>> emptyCollection = null,
            Func<IEnumerable<T>> all = null,
            Func<IQueryable<T>> asQueryable = null) {


            Key = (key.NotNull() ? key.Body.AsType<MemberExpression>().Member : null) ?? Key;
            New = newModel ?? New;
            DefaultKey = defaultKey ?? DefaultKey;
            KeysEqual = keysEqual ?? KeysEqual;
            EmptyCollection = emptyCollection ?? EmptyCollection;
            All = all ?? All;
            AsQueryable = asQueryable ?? AsQueryable;
        }

        
        protected static IEnumerable<T> EmptyArray()
        {
            return new T[] { };
        }
    }
}