using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace Harness.Framework {
    public static class TaskExtensions {
        public static Task EachAsync<T>(this IEnumerable<T> collection, Action<T> action) {
            return Task.Factory.StartNew(
                () => Task.WaitAll(
                    collection.Select(
                        x => x.AsTask(action)
                    ).ToArray()    
                )
            );
        }
        public static Task EachAsync<T>(this Task<IEnumerable<T>> collection, Action<T> action) 
        {
           return Task.Factory.StartNew(
                () => Task.WaitAll(
                    collection.AwaitResult().Select(
                        x => x.AsTask(action)
                    ).ToArray()
                )
            );
        }

        public static Task<IEnumerable<TY>> EachAsync<T, TY>(this Task<IEnumerable<T>> collection, Func<T, TY> action) {
            return Task<IEnumerable<TY>>.Factory.StartNew(() => {
                var results = new List<TY>();
                Task.WaitAll(
                    collection.AwaitResult().Select(
                        x => x.AsTask(y => results.Add(action(y)))
                    ).ToArray()
                );
                return results; 
            });
        }
        public static Task EachAsync<T, TY>(this IEnumerable<T> collection, Action<T, TY> action, TY context) {
            return Task.Factory.StartNew(
                () => Task.WaitAll(
                    collection.Select(
                        x => x.AsTask(y => action(y, context))
                    ).ToArray()    
                )
            );
        }

        public static Task EachAsync<T, TY>(this Task<IEnumerable<T>> collection, Action<T, TY> action, TY context) {
            return Task.Factory.StartNew(
                 () => Task.WaitAll(
                    (collection).EachAsync(
                        x => x.AsTask(y => action(y, context))
                    )
                )
            );
        }

        public static Task ProcessAsync<T>(this IEnumerable<T> collection, params Action<T>[] actions) {
            return collection.EachAsync(x => actions.EachAsync(y => y(x)).Await());
        }

        public static Task<T> ActionAsync<T>(this Task<T> task, Action<T> action) {



            return Task<T>.Factory.StartNew(() => {
                var t = task.AwaitResult();
                action(t);
                return t;
            });
           
        }

        public static Task<TY> FuncAsync<T, TY>(this Task<T> task, Func<T, TY> func) {
            
            return Task<TY>.Factory.StartNew(() => {
                var p = task.AwaitResult();
                func(p);
            });
        }

        public static Task<T> AsTask<T>(this T t) {
            return Task.Factory.StartNew<T>(() => t);
        }

        public static Task<TY> AsTask<T, TY>(this T t, Func<T, TY> func) {
            
            return Task.Factory.StartNew<TY>(x => func(t), t);

        }

        public static Task AsTask<T>(this T t, Action<T> action) {
            Task.Factory.StartNew(x => action(t), t);
        }

        

        public static void Await(this Task task) {
            if (task.Status == TaskStatus.Created)
                task.Wait();

        }

        public static T AwaitResult<T>(this Task<T> task) {
            task.Wait();
            return task.Result;
        }
    }
}