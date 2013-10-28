//using NuGet;

using System;
using System.Collections.Generic;
using System.Reflection;
using Autofac;

namespace Harness {
    public interface IEnvironment {
        IContainer Container { get; }
        IEnumerable<Assembly> AssemblyCache { get; }
        IEnumerable<Type> TypeCache { get; }
        TX Resolve<TX>();
        object Resolve(Type type);
        object Resolve(string typeName);
    }
}