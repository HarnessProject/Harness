// ***********************************************************************
// Assembly         : System.Extensions
// Author           : Nick Daniels
// Created          : 12-04-2013
//
// Last Modified By : Nick Daniels
// Last Modified On : 12-17-2013
// ***********************************************************************
// <copyright file="TypeExtensions.cs" company="">
//     . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Collections.Generic;
using System.Linq;

namespace System {
    public static class TypeExtensions {
       

        public static bool Is(this Type type, Type t) {
            return t.IsAssignableFrom(type);
        }

        public static bool Is<T>(this Type type) {
            return type.Is(typeof (T));
        }
        //public static bool Is(this Type type, Type t)
        //{
        //    return t.IsAssignableFrom(type);
        //}

        //public static bool Is<T>(this TypeInfo type)
        //{
        //    return type.Is(typeof(T).GetTypeInfo());
        //}
        public static T As<T>(this object obj) {
            return obj.Try(o => (T) o).Catch<Exception>((o, ex) => default(T)).Invoke();
        }
        public static T As<T>(this object obj, Action<T> initializer )
        {
            var t = obj.As<T>();
            initializer(t);
            return t;
        }
        public static bool Contains<T>(this IEnumerable<Type> enumerable) {
            return enumerable.Contains(typeof (T));
        }
    }
}