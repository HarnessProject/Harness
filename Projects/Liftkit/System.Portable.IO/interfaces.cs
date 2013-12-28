using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
 * Derived from Portable.IO
 * 
 */

namespace System.Portable.IO
{
    public enum FileAccess
    {
        Read, Write, ReadWrite
    }

    public interface IFileSystemElement
    {
        string Name { get; }
        string Path { get; }
        bool Exists { get; }
    }

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

    public interface IFile : IFileSystemElement
    {
        long Length { get; }
        Stream Open(FileAccess fileAccess);
        Task<Stream> OpenAsync(FileAccess fileAccess);
        void Delete();
        Task DeleteAsync();
    }

    public interface IDirectory : IFileSystemElement
    {
        long FileCount { get; }
        long DirectoryCount { get; }
        IFile CreateFile(string name);
        Task<IFile> CreateFileAsync(string name);
        IFile GetFile(string name);
        Task<IFile> GetFileAsync(string name);
        IEnumerable<IFile> GetFiles();
        Task<IEnumerable<IFile>> GetFilesAsync();
        IEnumerable<string> GetFileNames();
        Task<IEnumerable<string>> GetFileNamesAsync();
        IDirectory CreateDirectory(string name);
        Task<IDirectory> CreateDirectoryAsync(string name);
        IDirectory GetDirectory(string name);
        Task<IDirectory> GetDirectoryAsync(string name);
        IEnumerable<IDirectory> GetDirectories();
        Task<IEnumerable<IDirectory>> GetDirectoriesAsync();
        IEnumerable<string> GetDirectoryNames();
        Task<IEnumerable<string>> GetDirectoryNamesAsync();
        void Delete();
        Task DeleteAsync();
    }
}
