using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Foundation;
using Harness.Framework;
using Harness.WinRT;
using Newtonsoft.Json;

namespace Harness.Windows {
    /*
     * This is the Windows Runtime Component Version of Harness.
     * It can be consumed by all Windows Runtime Languages, but given we are using json for our proxy generation, Javascript is our primary target.
     * As you can see, to be consistent with the Windows Runtime, we use Windows in our namespace.
     * It's mostly to seperate thinking on components available to .Net on WinRT (Harness.WinRT) and Windows Runtime components (Harness.Windows).
     */

    public sealed class ProxyObject {
        private CultureInfo _currentCulture = CultureInfo.InvariantCulture;
        private JsonSerializer _json;

        internal ProxyObject(object obj) {
            _json = new JsonSerializer();

            WrappedObject = obj;
            WrappedType = WrappedObject.GetType();
            Properties = WrappedType.GetRuntimeProperties().Select(x => x.Name).ToArray();
            Methods = WrappedType.GetRuntimeMethods().Select(x => x.Name).ToArray();
            Fields = WrappedType.GetRuntimeFields().Select(x => x.Name).ToArray();
            Events = WrappedType.GetRuntimeEvents().Select(x => x.Name).ToArray();
        }

        private object WrappedObject { get; set; }
        private Type WrappedType { get; set; }

        public string[] Properties { get; private set; }
        public string[] Methods { get; private set; }
        public string[] Fields { get; private set; }
        public string[] Events { get; private set; }

        public string GetProperty(string name) {
            return
                WrappedType.Try(t => t.GetRuntimeProperty(name).GetValue(WrappedObject).ToJson())
                    .Catch<JsonSerializationException>((t, ex) => null)
                    .Catch<Exception>((t, ex) => JsonToken.Undefined.ToJson())
                    .Invoke();
        }

        public IAsyncOperation<string> GetPropertyAsync(string name) {
            return Task.Run(() => GetProperty(name)).AsAsyncOperation();
        }

        public bool SetProperty(string name, string val) {
            return
                WrappedType.Try(
                    t => {
                        t.GetRuntimeProperty(name).SetValue(WrappedObject, JsonValue.Parse(val).GetObject());
                        return true;
                    })
                    .Catch<Exception>((t, ex) => false)
                    .Invoke();
        }

        public IAsyncOperation<bool> SetProperyAsync(string name, string val) {
            return Task.Run(() => SetProperty(name, val)).AsAsyncOperation();
        }

        public string InvokeMethod(string name, string param) {
            return
                WrappedType.Try(
                    t =>
                        t.GetTypeInfo()
                        .GetDeclaredMethod(name)
                        .Invoke(WrappedObject, JsonArray.Parse(param).ToArray<object>())
                        .ToJson())
                    .Catch<Exception>((t, ex) => null)
                    .Finally(t => t.ToJson())
                    .Invoke();
        }

        public IAsyncOperation<string> InvokeMethodAsync(string name, string param) {
            return Task.Run(() => InvokeMethod(name, param)).AsAsyncOperation();
        }
    }
}