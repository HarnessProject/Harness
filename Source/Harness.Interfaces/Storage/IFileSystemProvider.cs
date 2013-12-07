namespace Harness.Storage {
    public interface IFileSystemProvider {
        IFileSystem FileSystem { get; set; }
        string MapPath(string subpath);
        bool PathExists(string subpath);
        bool FileExists(string filepath);
        void Initialize();
    }
}