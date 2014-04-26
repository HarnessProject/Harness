using PCLStorage;

namespace Harness.Framework.Interfaces {
    public interface IFileSystemLocations : IDependency {
        IFolder BaseDirectory { get; }
        IFolder LocalStorage { get; }
        IFolder RoamingStorage { get; }

    }
}