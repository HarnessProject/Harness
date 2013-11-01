using System;
using System.Collections.Generic;
using System.Linq;

namespace Harness.Framework {
    public static class TypeExtensions {
        public static bool CanBe<T>(this Type t) {
            return Q.If<Type>(t.Is).True(typeof (T));
        }

        public static bool CanBe(this Type type, Type t) {
            return Q.If<Type>(type.Is).True(t);
        }

        public static bool Is(this Type type, Type t) {
            return t.IsAssignableFrom(type);
        }

        public static bool Is<T>(this Type type) {
            return typeof (T).Is(type);
        }

        public static T As<T>(this object obj) where T : class {
            return obj as T;
        }

        public static bool Contains<T>(this IEnumerable<Type> enumerable) {
            return enumerable.Contains(typeof (T));
        }
    }
}