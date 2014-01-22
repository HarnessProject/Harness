using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace System.Portable.IO
{
    public class RuntimeFileSystemElement : IFileSystemElement
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool Exists { get; set; }
    }

    public class RuntimeFile : RuntimeFileSystemElement, IFile
    {
        public long Length { get; private set; }
        public Stream Open(FileAccess fileAccess)
        {
            Stream r = null;
            switch (fileAccess)
            {
                case FileAccess.Read:
                    r = File.Open(Path, FileMode.Open, System.IO.FileAccess.Read);
                    break;
                case FileAccess.ReadWrite:
                    r = File.Open(Path, FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);
                    break;
                case FileAccess.Write:
                    r = File.Open(Path, FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                    break;
                default:
                    r = null;
                    break;
            }
            return r;
        }

        public Task<Stream> OpenAsync(FileAccess fileAccess)
        {
            return this.AsTask(x => Open(fileAccess));
        }

        public void Delete()
        {
            File.Delete(Path);
        }

        public Task DeleteAsync()
        {
            return this.AsTask(x => Delete());
        }

        public static explicit operator RuntimeFile(FileInfo file)
        {
            var f = new RuntimeFile
            {
                Path = file.FullName,
                Name = file.Name,
                Exists = File.Exists(file.FullName),
                Length = file.Length
                
            };
            return f;
        }

        public static explicit operator RuntimeFile(string file) {
            return new FileInfo(file).As<RuntimeFile>();
        }
    }

public class RuntimeDirectory : RuntimeFileSystemElement, IDirectory {
    public RuntimeDirectory() {
        
    }
    
    protected FileInfo GetFileInfo(string name) {
        return new FileInfo(System.IO.Path.Combine(Path,name));
    }
    protected DirectoryInfo GetDirectoryInfo(string name)
    {
        return new DirectoryInfo(System.IO.Path.Combine(Path, name));
    }
    public long FileCount { get; private set; }
    public long DirectoryCount { get; private set; }

    public IFile CreateFile(string name) {
        var f = GetFileInfo(name);
        if (!f.Exists) f.Create();
        return f.As<IFile>();
    }

    public Task<IFile> CreateFileAsync(string name) {
        return this.AsTask(x => CreateFile(name));
    }

    public IFile GetFile(string name) {
        return GetFileInfo(name).As<IFile>();
    }

    public Task<IFile> GetFileAsync(string name) {
        return this.AsTask(x => GetFile(name));
    }

    public IEnumerable<IFile> GetFiles() {
        return Directory.GetFiles(Path).Select(x => GetFileInfo(x).As<RuntimeFile>());
    }

    public Task<IEnumerable<IFile>> GetFilesAsync() {
        return this.AsTask(x => GetFiles());
    }

    public IEnumerable<string> GetFileNames() {
        return GetFiles().Select(x => x.Name);
    }

    public Task<IEnumerable<string>> GetFileNamesAsync() {
        return this.AsTask(x => GetFileNames());
    }

    public IDirectory CreateDirectory(string name) {
        var r = GetDirectoryInfo(name);
        if (!r.Exists) r.Create();
        return r.As<IDirectory>();
    }

    public Task<IDirectory> CreateDirectoryAsync(string name) {
        return this.AsTask(x => CreateDirectory(name));
    }

    public IDirectory GetDirectory(string name) {
        return GetDirectoryInfo(name).As<IDirectory>();
    }

    public Task<IDirectory> GetDirectoryAsync(string name) {
        return this.AsTask(x => GetDirectory(name));
    }

    public IEnumerable<IDirectory> GetDirectories() {
        return Directory.GetDirectories(Path).Select(x => GetDirectoryInfo(x).As<RuntimeDirectory>());
    }

    public Task<IEnumerable<IDirectory>> GetDirectoriesAsync() {
        return this.AsTask(x => GetDirectories());
    }

    public IEnumerable<string> GetDirectoryNames() {
        return GetDirectories().Select(x => x.Name);
    }

    public Task<IEnumerable<string>> GetDirectoryNamesAsync() {
        return this.AsTask(x => GetDirectoryNames());
    }

    public void Delete() {
        Directory.Delete(Path);
    }

    public Task DeleteAsync() {
        return this.AsTask(x => Delete());
    }


    public static explicit operator RuntimeDirectory(DirectoryInfo directory) {
        var r = new RuntimeDirectory {
            DirectoryCount = directory.EnumerateDirectories().LongCount(), 
            Exists = directory.Exists, 
            FileCount = directory.EnumerateFiles().LongCount(), 
            Name = directory.Name, 
            Path = directory.FullName
        };
        return r;
    }

    public static explicit operator RuntimeDirectory(string path) {
        return new DirectoryInfo(path).As<RuntimeDirectory>();
    }
}

public class RuntimeFileSystem : IFileSystem
    {
        public IDirectory AppDirectory { get { return AppDomain.CurrentDomain.BaseDirectory.As<IDirectory>(); } }

        public IFile GetFile(string path) {
            return path.As<RuntimeFile>();
        }

        public Task<IFile> GetFileAsync(string path) {
            return this.AsTask(x => GetFile(path));
        }

        public bool ExistsFile(string path) {
            return File.Exists(path);
        }

        public Task<bool> ExistsFileAsync(string path) {
            return this.AsTask(x => ExistsFile(path));
        }

        public IDirectory GetDirectory(string path) {
            return path.As<RuntimeDirectory>();
        }

        public Task<IDirectory> GetDirectoryAsync(string path) {
            return this.AsTask(x => GetDirectory(path));
        }

        public bool ExistsDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsDirectoryAsync(string path)
        {
            throw new NotImplementedException();
        }
    }
}