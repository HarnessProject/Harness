using System;
using System.Threading.Tasks;
using Windows.Foundation;
using Harness.Core;
using Environment = Harness.WinRT.Environment;

namespace Harness.Windows {
    public sealed class Proxy {
        public Proxy() {
            Application<Environment>.New(new Environment());
        }

        public ProxyObject Resolve(string type) {
            return new ProxyObject(Application.Resolve(type));
        }

        public IAsyncOperation<ProxyObject> ResolveAsync(string type) {
            return WindowsRuntimeSystemExtensions.AsAsyncOperation(Task.Run(() => Resolve(type)));
        }
    }
}