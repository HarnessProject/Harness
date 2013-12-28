using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System.Threading.Tasks {
    public static class TaskExtensions {
        public static async Task EachAsync<T>(this IEnumerable<T> collection, Action<T> action) {
            await collection.AsTask(
                c => Task.WaitAll(
                    c.Select(
                        x => x.AsTask(action)
                    ).ToArray()    
                )
            );
        }
        public static async Task EachAsync<T>(this Task<IEnumerable<T>> collection, Action<T> action) {
            await (await collection).EachAsync(action);
        }

        public static async Task<IQueryable<TY>> SelectAsync<T, TY>(this Task<IEnumerable<T>> collection, Func<T, TY> action) {
            return await (await collection).AsTask(c => c.Select(action).AsQueryable());
        }
        public static Task EachAsync<T, TY>(this IEnumerable<T> collection, Action<T, TY> action, TY context) {
            var a = action;
            var h = ((Expression<Action<T>>)  (y => a(y, context) )).Compile();
            var i = ((Expression<Action<T>>) ( y => h(y) )).Compile();
            var t = ((Expression<Func<T,Task>>) ( x => x.AsTask(i) )).Compile();
            return collection.AsTask(c => Task.WaitAll(c.Select(t).ToArray()));
        }

        public static async Task EachAsync<T, TY>(this Task<IEnumerable<T>> collection, Action<T, TY> action, TY context) {
            await EachAsync(await collection, action, context);
        }

        public static async Task ProcessAsync<T>(this IEnumerable<T> collection, params Action<T>[] actions) {
            await collection.EachAsync(async x => await actions.EachAsync(y => y(x)));
        }

        public static async Task ActionAsync<T>(this Task<T> task, Action<T> action) {
            await (await task).AsTask(action);
        }

        public static async Task<TY> FuncAsync<T, TY>(this Task<T> task, Func<T, TY> func) {
            return await (await task).AsTask(func);

        }

        public static Task<T> AsTask<T>(this T t) {
            return Task.Factory.StartNew(() => t);
        }

        public static Task<TY> AsTask<T, TY>(this T t, Func<T, TY> func) {
            return Task.Factory.StartNew(x => func(x.As<T>()) , t);

        }

        public static Task AsTask<T>(this T t, Action<T> action) {
           
            return Task.Factory.StartNew(x => action(x.As<T>()), t);
        }

        

        public static void Await(this Task task) {
            Task.WaitAll(new Task[] {task});
        }

        public static T AwaitResult<T>(this Task<T> task) {
            task.Await();
            return task.Result;
        }
    }
}