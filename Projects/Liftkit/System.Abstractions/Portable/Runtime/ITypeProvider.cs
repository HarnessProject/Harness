using System.Collections.Generic;
using System.Reflection;

namespace System.Portable.Runtime {
    public interface ITypeProvider {
        IEnumerable<Assembly> Assemblies { get; }
        IEnumerable<Type> Types { get; }

       
    }
}