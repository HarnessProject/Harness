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

       

        // ONLY TO BE USED FOR TESTING PURPOSES
        // Makes a one liner end in whatever value you want.
        // It's lazy, and shouldn't be used in production code.
        public static T Return<T>(this Object o, T val = default (T)) {
            return val;
        } 

        //There is no net here...
        public static void ResolveAndInvoke(this Object o, MethodInfo method) {
            var param = new List<Object>();
            method.GetParameters().Each(x => param.Add(X.ServiceLocator.GetInstance(x.ParameterType)));
            method.Invoke(o, param.ToArray());
        }
        public static void ResolveAndInvoke(this Object o, string methodName)
        {
            var method = o.GetType().GetMethod(methodName);
            if (method == null) return;
            ResolveAndInvoke(o, method);
        }

        public static Task ResolveAndInvokeAsync(this Object o, MethodInfo method)
        {
            var param = new List<Object>();
            
            return
            method
            .GetParameters()
            .As<IEnumerable<ParameterInfo>>()
            .AsTask()
            .EachAsync(
                y => param.Add(X.ServiceLocator.GetInstance(y.ParameterType)))
            .ContinueWith(
                x => method.Invoke(o, param.ToArray())
            );
            
        }
        public static Task ResolveAndInvokeAsync(this Object o, string methodName)
        {
            var method = o.GetType().GetMethod(methodName);
            return method == null ? Task.Factory.StartNew(() => {}) : o.AsTask(async i => await i.ResolveAndInvokeAsync(method));
        }
    }
}