using System.Portable.Runtime;
using Caliburn.Micro;

/* Interfaces for our auto injection scheme... */

namespace System.Composition.CaliburnMicro {
    public interface IShell : IConductor, ISingletonDependency {} // App shell
    
    public interface IModelContainer : IConductor, IDependency {} //Container
    
}