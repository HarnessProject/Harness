using System.Threading.Tasks;

namespace System.Portable.IO {
    public interface IFileSystem
    {
        IDirectory AppDirectory { get; }
        IFile GetFile(string path);
        Task<IFile> GetFileAsync(string path);
        bool ExistsFile(string path);
        Task<bool> ExistsFileAsync(string path);
        IDirectory GetDirectory(string path);
        Task<IDirectory> GetDirectoryAsync(string path);
        bool ExistsDirectory(string path);
        Task<bool> ExistsDirectoryAsync(string path);
    }
}