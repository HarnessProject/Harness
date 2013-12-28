using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;


namespace System.Collections.Generic {
    public static class EnumerableExtensions {
        public static async void Each<T>(this IEnumerable<T> collection, Action<T> action) {
            await collection.EachAsync(action);
        }

        public static void Each<T>(this IEnumerable<T> collection, params Action<T>[] actions) {
            actions.Each(x => collection.Each(x));
        }

        public static void Each<T, TY>(this IEnumerable<T> collection, TY state, params Action<T,TY>[] actions) {
            actions.Each(x => collection.Each(x,state));
        }

        public static void Each<T, TY>(this IEnumerable<T> collection, Action<T,TY> action, TY state) {
        
            collection.Each(x => action(x,state));
        }
    }
}