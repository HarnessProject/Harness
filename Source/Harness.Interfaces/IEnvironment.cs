//using NuGet;

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Harness.Storage;

namespace Harness {
    public interface IEnvironment {
        IEnumerable<Type> GetTypes(Func<Type, bool> predicate = null);
        IEnumerable<IFileSystemProvider> GetFileSystems(Func<IFileSystem, bool> predicate = null);

        Task InitializeAsync();
        void Initialize();
    }
}