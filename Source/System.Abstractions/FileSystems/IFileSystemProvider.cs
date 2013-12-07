using System.Composition;

namespace System.FileSystems {
    public interface IFileSystemProvider : ISingletonDependency {
        bool ProviderFor(string url);
        IFileSystem FileSystemFor(string url);
    }
}