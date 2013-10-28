using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Harness.Services;

namespace Harness {
    public static class Extensions {
        public static IEnumerable<T> EvaluateAll<T, TY>(this IEnumerable<IServiceProvider<T>> collection, TY context) {
            return
                collection
                    .Select(i => i.Evaluate(context))
                    .Where(er => er.IsMatch)
                    .OrderByDescending(er => er.Priority)
                    .Select(er => er.Service);
        }

        public static object InvokeWithNamedParameters(
            this MethodBase self, object obj,
            IDictionary<string, object> namedParameters) {
            return self.Invoke(obj, MapParameters(self, namedParameters));
        }

        public static object[] MapParameters(this MethodBase method, IDictionary<string, object> namedParameters) {
            string[] paramNames = method.GetParameters().Select(p => p.Name).ToArray();
            var parameters = new object[paramNames.Length];

            for (int i = 0; i < parameters.Length; ++i) {
                parameters[i] = Type.Missing;
            }
            foreach (var item in namedParameters) {
                string paramName = item.Key;
                int paramIndex = Array.IndexOf(paramNames, paramName);
                parameters[paramIndex] = item.Value;
            }
            return parameters;
        }
    }
}