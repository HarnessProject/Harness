using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace System.Portable.IO
{
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
            return GetFileInfo(name).As<RuntimeFile>();
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

        public static implicit operator RuntimeDirectory(DirectoryInfo directory)
        {
            if (!directory.Exists) throw new DirectoryNotFoundException("The Directory specified does not exist : " + directory.FullName);
            var dirCount = directory.EnumerateDirectories().LongCount();
            var fileCount = directory.EnumerateFiles().LongCount();
        
            var r = new RuntimeDirectory {
                DirectoryCount = dirCount, 
                Exists = directory.Exists, 
                FileCount = fileCount, 
                Name = directory.Name, 
                Path = directory.FullName
            };

            return r;
        }

        public static implicit operator RuntimeDirectory(string path) {
            return new DirectoryInfo(path).As<RuntimeDirectory>();
        }
    }
}