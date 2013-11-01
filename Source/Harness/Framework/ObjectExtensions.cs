using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Harness.Framework {
    public static class ObjectExtensions {
        public static T Action<T>(this T obj, params Action<T>[] actions) {
            actions.Each(x => x(obj));
            return obj;
        }

        public static Dictionary<string, object> ToDictionary(this object obj) {
            var p = new Dictionary<string, object>();
            if (obj != null) {
                p = obj.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public).ToDictionary(k => k.Name, v => v.GetValue(obj, null));
            }

            return p;
        }
    }
}