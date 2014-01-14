using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace System {
    public static class ObjectExtensions {

        

        public static T Action<T>(this T obj, params Action<T>[] actions) {
            actions.Each(x => x(obj));
            return obj;
        }

        public static TY Func<T, TY>(this T obj, Func<T, TY> func) {
            return func(obj);
        }

        public static bool NotNull<T>(this T o) where T : class {
            return o != null;
        }

        public static T NotNull<T>(this T o, Action<T> action) where T : class {
            var b = o != null;
            if (b) action(o);
            return o;
        }

        public static bool NotDefault<T>(this T o) {
            return !EqualityComparer<T>.Default.Equals(o, default(T));
        }

        public static bool IsNull<T>(this T o) where T : class {
            return o == null;
        }

        public static T IsNull<T>(this T o, Func<T> initializer) {
            return initializer();
        }

        // ONLY TO BE USED FOR TESTING PURPOSES
        // Makes a one liner end in whatever value you want.
        // It's lazy, and shouldn't be used in production code.
        public static T Return<T>(this Object o, T val = default (T)) {
            return val;
        }


    }
}