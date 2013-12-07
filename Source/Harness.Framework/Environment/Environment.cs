using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Harness.Storage;

namespace Harness
{
    public abstract class EnvironmentBase : IEnvironment, ISingletonDependency
    {
        public abstract IEnumerable<Type> GetTypes(Func<Type, bool> predicate = null);
        public abstract IEnumerable<IFileSystemProvider> GetFileSystems(Func<IFileSystem, bool> predicate = null);
        public abstract Task InitializeAsync();
        public abstract void Initialize();

        public CompositeFileSystem FileSystem() {
            return new CompositeFileSystem(GetFileSystems());
        }
    }
}
