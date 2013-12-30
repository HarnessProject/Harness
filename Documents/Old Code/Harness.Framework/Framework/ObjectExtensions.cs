using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;


namespace Harness.Framework {
    public static class ObjectExtensions {
        public static T Action<T>(this T obj, params Action<T>[] actions) {
            actions.Each(x => x(obj));
            return obj;
        }

        public static bool NotNull<T>(this T o) where T : class {
            return o != null;
        } 

        // ONLY TO BE USED FOR TESTING PURPOSES
        // Makes a one liner end in whatever value you want.
        // It's lazy, and shouldn't be used in production code.
        public static T Return<T>(this Object o, T val = default (T)) {
            return val;
        } 

       
}