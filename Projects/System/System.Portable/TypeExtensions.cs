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
using System.Contracts;
using System.Diagnostics.Contracts;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Portable;
using System.Portable.Runtime;
using System.Portable.Runtime.Reflection;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;

#endregion

namespace System {
    public static class TypeExtensions {
       
        private static TY ExpressionConvert<T,TY>(object source) {
            
            //This fixes a bug in the .Net Framework - Enables casting from object to a compatible type in a PCL
            var p = Expression.Parameter(typeof (T));
            var converted = Expression.Convert(p, typeof(TY));
            var m = Expression.Lambda<Func<T, TY>>(converted, p).Compile();
            return m((T)source);
        }

        

        private static TY ReflectedConvert<TY>(object o)
        {
            //This for some UNKNOWN reason is faster than just calling ExpressionConvert - ?? 
            var m = typeof(TypeExtensions).GetMethods(BindingFlags.Static | BindingFlags.NonPublic).FirstOrDefault(x => x.Name.Contains("ExpressionConvert"));
            return (TY)m.MakeGenericMethod(o.GetType(), typeof(TY)).Invoke(null, new object[] { o });

        }
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

        //public static bool Is<T>(this TypeInfo type)
        //{
        //    return type.Is(typeof(T).GetTypeInfo());
        //}
        

        public static MethodInfo GetMethod<TY>(this Expression<Func<TY>> d)
        {
            var m = (MethodCallExpression)d.Body;
            return m.Method;
        }

        public static TY As<TY>(this object obj) {
           
            return ReflectedConvert<TY>(obj);
        }

        public static TY As<TY>(this object obj, Action<TY> initializer) where TY : class {
            var t = obj.As<TY>();
            initializer(t);
           
            return t;
        }

        public static bool Contains<T>(this IEnumerable<Type> enumerable) {
            return enumerable.Contains(typeof (T));
        }
    }
}