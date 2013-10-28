using System;
using System.IO;
using System.Security;
using System.Threading.Tasks;
using Windows.Foundation;
using Harness.Framework;
using Newtonsoft.Json;

namespace Harness.WinRT {
    public static class Extensions {
        [SecurityCritical]
        public static T Wait<T>(this IAsyncOperation<T> op) {
            Task<T> task = op.AsTask();
            return task.AwaitResult();
        }

        public static string ToJson(this object obj) {
            var jserializer = new JsonSerializer();
            var writer = new StringWriter();
            jserializer.Serialize(writer, obj);
            return writer.ToString();
        }
    }
}