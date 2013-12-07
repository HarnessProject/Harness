using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Harness.Framework;

// Code is Framework for Async platforms...
// ReSharper disable once CheckNamespace
namespace Harness.Framework
{
    public static class ReflectionExtensions
    {
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
            return method == null ? Task.Factory.StartNew(() => { }) : o.AsTask(async i => await i.ResolveAndInvokeAsync(method));
        }
    }
}
