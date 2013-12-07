using System.Collections.Generic;
using System.Linq;

namespace System {
    public static class TypeExtensions {
        public static bool CanBe<T>(this Type t) {
           
            return Determine.If<Type>(t.Is).Result(typeof (T));
        }

        public static bool CanBe(this Type type, Type t) {
            return Determine.If<Type>(type.Is).Result(t);
        }

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
            return (T)obj;
        }
        public static T As<T>(this object obj, Action<T> initializer )
        {
            var t = (T)obj;
            initializer(t);
            return t;
        }
        public static bool Contains<T>(this IEnumerable<Type> enumerable) {
            return enumerable.Contains(typeof (T));
        }
    }
}