using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using RuntimePath = System.IO.Path;

namespace System.Portable.IO
{
    public class RuntimeDirectory : RuntimeFileSystemElement, IDirectory {
        
        protected FileInfo GetFileInfo(string name) {
            return new FileInfo(Combine(name));
        }
        protected DirectoryInfo GetDirectoryInfo(string name)
        {
            return new DirectoryInfo(Combine(name));
        }
        public long FileCount { get { return Directory.EnumerateFiles(Path).Count(); } }
        public long DirectoryCount { get { return Directory.EnumerateDirectories(Path).Count(); } }

        public IFile CreateFile(string name) {
            var path = Combine(name);
            File.Exists(path).True( () => FileSystemException.FileExists(path).Throw() );
            File.Create(path);
            return new RuntimeFile { Path = path };
        }

        public Task<IFile> CreateFileAsync(string name) {
            return this.AsTask(x => CreateFile(name));
        }

        public IFile GetFile(string name) {
            var path = Combine(name);
            File.Exists(path)
                .False(() => FileSystemException.FileNotFound(path));
            return new RuntimeFile { Path = path };
        }

        public Task<IFile> GetFileAsync(string name) {
            return this.AsTask(x => GetFile(name));
        }

        public IEnumerable<IFile> GetFiles() {
            return 
            Directory.GetFiles(Path)
                     .Select(x => new RuntimeFile { Path = x });
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
            var path = Combine(name);
            Directory.Exists(path)
                     .True( () => FileSystemException.DirectoryExists(path).Throw() );

            Directory.CreateDirectory(path);
            return new RuntimeDirectory() {Path = path};
        }

        public Task<IDirectory> CreateDirectoryAsync(string name) {
            return this.AsTask(x => CreateDirectory(name));
        }

        public IDirectory GetDirectory(string name) {
            var path = Combine(name);
            Directory.Exists(path)
                .False( () => FileSystemException.DirectoryNotFound(path).Throw() );
            return new RuntimeDirectory {Path = path};
        }

        protected string Combine(string name) {
            return RuntimePath.Combine(Path, name);
        }

        public Task<IDirectory> GetDirectoryAsync(string name) {
            return this.AsTask(x => GetDirectory(name));
        }

        public IEnumerable<IDirectory> GetDirectories() {
            return 
            Directory.GetDirectories(Path)
            .Select(x => new RuntimeDirectory() { Path = x });
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

        public static implicit operator RuntimeDirectory(DirectoryInfo directory) {
            
            directory.Exists
                .False(
                    () => 
                        FileSystemException
                        .DirectoryNotFound(directory.FullName)
                        .Throw()
                );

            return new RuntimeDirectory { Path = directory.FullName };
        }

        public static implicit operator RuntimeDirectory(string path) {
            return new RuntimeDirectory() { Path = path } ;
        }


        public override bool Exists {
            get { return Directory.Exists(Path); }
            

        }
    }
}