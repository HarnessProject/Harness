using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace System.Collections
{
    public static class EnumerationExtensions
    {
        public static IEnumerable<T> WhereIs<T>(this IEnumerable collection) {
            return collection.Cast<T>().WhereNotDefault();
        }

        public static IEnumerable<T> WhereNotDefault<T>(this IEnumerable<T> collection) {
            return collection.Where(x => x.NotDefault());
        } 

        public static async void Each<T>(this IEnumerable collection, Action<T> action) {
            await collection.WhereIs<T>().EachAsync(action);
        }

        public static void Each<T>(this IEnumerable collection, params Action<T>[] actions)
        {
            actions.Each(collection.Each);
        }

        public static void Each<T, TY>(this IEnumerable collection, TY state, params Action<T, TY>[] actions)
        {
            actions.Each(x => collection.Each(x,state));
        }

        public static void Each<T, TY>(this IEnumerable collection, Action<T, TY> action, TY state) {
            
            collection.Each<T>(i => action(i,state));
        }

        
    }
}
