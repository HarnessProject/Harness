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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Harness.Framework.Extensions {
    public static class TypeExtensions {
        public static bool Is(this object o, Type t) {
            return o.GetType().Is(t);
        }

        public static bool Is<T>(this object o) {
            return o.GetType().Is<T>();
        }

        public static bool Is(this Type type, Type t) {
            return t.IsAssignableFrom(type);
        }

        public static bool Is<T>(this Type type) {
            return type.Is(typeof (T));
        }

        public static bool CouldBe(this Type type, Type t) {

            var isGeneric = Filter.If<Type>(x => x.IsGenericType);
            var isGenericDef = Filter.If<Type>(x => x.IsGenericTypeDefinition);

            var typeGeneric = isGeneric(type);
            var typeDef = isGenericDef(type);
            var tGeneric = isGeneric(t);
            var tDef = isGenericDef(t);

            var bothGeneric = typeGeneric && tGeneric;
            var bothDef = typeDef && tDef;
            var bothNeither = !bothGeneric && !bothDef;

            if (bothNeither) return type.Is(t);
            
            if (typeGeneric && tDef) return type.GetGenericTypeDefinition().Is(t);
            if (typeDef && tGeneric) return type.Is(t.GetGenericTypeDefinition());
            
            return type.Is(t);

        }

        public static bool CouldBe<T>(this Type type) {
            return CouldBe(type, typeof (T));
        }

        //public static bool Is<T>(this TypeInfo type)
        //{
        //    return type.Is(typeof(T).GetTypeInfo());
        //}

        public static MethodInfo GetMethod<TY>(this Expression<Func<TY>> d) {
            var m = (MethodCallExpression) d.Body;
            return m.Method;
        }

        public static TY AsType<TY>(this object obj) {
            return DomainProvider.Instance.Cast<TY>(obj);
        }

        public static TY AsType<TY>(this object obj, Action<TY> initializer) where TY : class {
            var t = obj.AsType<TY>();
            initializer(t);
            return t;
        }

        public static bool Contains<T>(this IEnumerable<Type> enumerable) {
            return enumerable.Contains(typeof (T));
        }

        public static object CreateInstance(this Type type, params object[] args) {
            return Provider.Reflector.CreateInstance(type, args);
        }

        public static T StaticInstance<T>(this Type type) {
            var prop = type.GetProperties(BindingFlags.Static).FirstOrDefault(p => p.Name.Contains("Instance") && p.PropertyType == typeof(T));
            return prop.IsNull() ? default(T) : prop.GetValue(null, null).AsType<T>();

        }
    }
}