using System;
using System.Runtime.Environment;

namespace Harness.Http {
    public class HttpTypeProvider : TypeProvider {
        public HttpTypeProvider() : base(AppDomain.CurrentDomain.BaseDirectory) { }
    }
}