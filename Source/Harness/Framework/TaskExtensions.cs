using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Runtime.CompilerServices;

namespace Harness.Framework {
    public static class TaskExtensions {
        public static async Task EachAsync<T>(this IEnumerable<T> collection, Action<T> action) {
            await Task.Factory.StartNew(
                () => Task.WaitAll(
                    collection.Select(
                        x => x.AsTask(action)
                    ).ToArray()    
                )
            );
        }
        public static async Task EachAsync<T>(this Task<IEnumerable<T>> collection, Action<T> action) 
        {
            await Task.Factory.StartNew(
                () => Task.WaitAll(
                    collection.AwaitResult().Select(
                        x => x.AsTask(action)
                    ).ToArray()
                )
            );
        }
        public static async Task<IEnumerable<TY>> EachAsync<T,TY>(this Task<IEnumerable<T>> collection, Func<T,TY> action) {
            IList<TY> results = new List<TY>();
            await (await collection).Select(
                x => x.AsTask(y => results.Add(action(y)))
            ).EachAsync(x => x.Await());
            return results;
        }
        public static async Task EachAsync<T, TY>(this IEnumerable<T> collection, Action<T, TY> action, TY context) {
            await Task.Factory.StartNew(
                () => Task.WaitAll(
                    collection.Select(
                        x => x.AsTask(y => action(y, context))
                    ).ToArray()    
                )
            );
        }

        public static async Task EachAsync<T, TY>(this Task<IEnumerable<T>> collection, Action<T, TY> action, TY context) {
            await Task.Factory.StartNew(
                async () => Task.WaitAll(
                    (await collection).Select(
                        x => x.AsTask(y => action(y, context))
                    ).ToArray()    
                )
            );
        }

        public static async Task ProcessAsync<T>(this IEnumerable<T> collection, params Action<T>[] actions) {
            await collection.EachAsync(x => actions.EachAsync(y => y(x)));
        }

        public static async Task<T> ActionAsync<T>(this Task<T> task, Action<T> action) {

            T t = await task;
            action(t);
            return t;
           
        }

        public static async Task<TY> FuncAsync<T, TY>(this Task<T> task, Func<T, TY> func) {
            var p = await task;
            return func(p);
        }

        public static async Task<T> AsTask<T>(this T t) {
            return await Task.Factory.StartNew<T>(() => t);
        }

        public static async Task<TY> AsTask<T, TY>(this T t, Func<T, TY> func) {
            
            return await Task.Factory.StartNew<TY>(x => func(t), t);

        }

        public static async Task AsTask<T>(this T t, Action<T> action) {
            await Task.Factory.StartNew(x => action(t), t);
        }

        

        public static void Await(this Task task) {
            task.Wait();
        }

        public static T AwaitResult<T>(this Task<T> task) {
            task.Wait();
            return task.Result;
        }
    }
}