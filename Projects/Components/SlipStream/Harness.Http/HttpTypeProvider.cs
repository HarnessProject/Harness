using System;
using System.Portable.Runtime;

namespace Harness.Http {
    public class HttpTypeProvider : TypeProvider {
        public HttpTypeProvider() : base(AppDomain.CurrentDomain.BaseDirectory) { }
    }
}