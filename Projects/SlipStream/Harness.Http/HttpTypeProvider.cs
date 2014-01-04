using System;
using System.Portable.Runtime.Environment;

namespace Harness.Http {
    public class HttpTypeProvider : TypeProvider {
        public HttpTypeProvider() : base(AppDomain.CurrentDomain.BaseDirectory) { }
    }
}