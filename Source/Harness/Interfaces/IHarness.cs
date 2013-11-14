//using NuGet;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;


namespace Harness {
    public interface IEnvironment {
        IEnumerable<Assembly> AssemblyCache { get; }
        IEnumerable<Type> TypeCache { get; }
        bool IsReady { get; }

        Task Initialize(IServiceLocator locator);
    }

    public interface IServiceLocator : Microsoft.Practices.ServiceLocation.IServiceLocator,IDisposable {
        bool GetImplementation<T>(Action<T> action) where T : IServiceLocator;
    }

    

    
}