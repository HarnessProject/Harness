using System;
using System.Collections.Generic;

namespace Harness.Framework {
    public static class EnumerableExtensions {
        public static void Each<T>(this IEnumerable<T> collection, Action<T> action) {
            //Calling Each WILL project your collection...
            foreach (var i in collection) action(i);
        }

        public static void Each<T>(this IEnumerable<T> collection, params Action<T>[] actions) {
            collection.Each(i => actions.Each(x => x(i)));
        }

        public static void Each<T, TY>(this IEnumerable<T> collection, TY state, params Action<T, TY>[] actions) {
            collection.Each(i => actions.Each(x => x(i, state)));
        }

        public static void Each<T, TY>(this IEnumerable<T> collection, Action<T, TY> action, TY state) {
            collection.Each(i => action(i, state));
        }
    }
}