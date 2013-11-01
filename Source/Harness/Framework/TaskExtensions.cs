using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Harness.Framework {
    public static class TaskExtensions {
        public static Task EachAsync<T>(this IEnumerable<T> collection, Action<T> action) {
            return new Task(() => Task.WaitAll(collection.Select(i => Task.Factory.StartNew(() => action(i))).ToArray()));
        }

        public static Task EachAsync<T, TY>(this IEnumerable<T> collection, Action<T, TY> action, TY context) {
            return new Task(() => Task.WaitAll(collection.Select(i => Task.Factory.StartNew(() => action(i, context))).ToArray()));
        }

        public static Task ProcessAsync<T>(this IEnumerable<T> collection, params Action<T>[] actions) {
            return new Task(() => collection.EachAsync(i => actions.EachAsync(a => a(i)).Await()).Await());
        }

        public static Task<T> ActionAsync<T>(this Task<T> task, Action<T> action) {
            return new Task<T>(() => {
                T t = task.AwaitResult();
                action(t);
                return t;
            });
        }

        public static Task<Ty> FuncAsync<T, Ty>(this Task<T> task, Func<T, Ty> func) {
            return new Task<Ty>(() => func(task.AwaitResult()));
        }

        public static Task<T> AsTask<T>(this T t) {
            return new Task<T>(() => t);
        }

        public static Task Begin(this Task task) {
            if (task.Status == TaskStatus.Created) task.Start();
            return task;
        }

        public static void Await(this Task task) {
            task.Begin().Wait();
        }

        public static T AwaitResult<T>(this Task<T> task) {
            task.Begin().Wait();
            return task.Result;
        }
    }
}