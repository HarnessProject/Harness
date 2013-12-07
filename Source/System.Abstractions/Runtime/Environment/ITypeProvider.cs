using System.Collections.Generic;
using System.Composition;
using System.Reflection;

namespace System.Runtime.Environment {
    public interface ITypeProvider {
        IEnumerable<Assembly> Assemblies { get; }
        IEnumerable<Type> Types { get; }

       
    }
}