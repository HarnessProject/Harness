using System;
using System.Collections.Generic;
using System.IO;
using System.Portable.IO;
using System.Portable.Runtime;
using System.Runtime.Environment;
using System.Threading.Tasks;

namespace System.Portable.IO
{
    public class PhysicalFileSystemElement : IFileSystemElement
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool Exists { get; set; }
    }

    public class PhysicalFile : PhysicalFileSystemElement, IFile
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

        public static explicit operator PhysicalFile(FileInfo file)
        {
            var f = new PhysicalFile
            {
                Path = file.FullName,
                Name = file.Name,
                Exists = File.Exists(file.FullName),
                Length = file.Length
                
            }};
        }
    }

    public class PhysicalDirectory : PhysicalFileSystemElement, IDirectory
    {
        protected FileInfo GetFileInfo(string name)
        {
            return new FileInfo(Path + System.IO.Path.DirectorySeparatorChar + name);
        }
        
        public long FileCount { get; private set; }
        public long DirectoryCount { get; private set; }
        public IFile CreateFile(string name)
        {
            var f = GetFileInfo() 
            f.Create();
            return f.As<PhysicalFile>();
        }

        public Task<IFile> CreateFileAsync(string name)
        {
            return this.AsTask(x => CreateFile(name))
        }

        public IFile GetFile(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IFile> GetFileAsync(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IFile> GetFiles()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IFile>> GetFilesAsync()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetFileNames()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetFileNamesAsync()
        {
            throw new NotImplementedException();
        }

        public IDirectory CreateDirectory(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IDirectory> CreateDirectoryAsync(string name)
        {
            throw new NotImplementedException();
        }

        public IDirectory GetDirectory(string name)
        {
            throw new NotImplementedException();
        }

        public Task<IDirectory> GetDirectoryAsync(string name)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<IDirectory> GetDirectories()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IDirectory>> GetDirectoriesAsync()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<string> GetDirectoryNames()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetDirectoryNamesAsync()
        {
            throw new NotImplementedException();
        }

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync()
        {
            throw new NotImplementedException();
        }
    }

    public class PhysicalFileSystem : IFileSystem
    {
        public IDirectory AppDirectory { get { return AppDomain.CurrentDomain.BaseDirectory; }
        public IFile GetFile(string path)
        {
            throw new NotImplementedException();
        }

        public Task<IFile> GetFileAsync(string path)
        {
            throw new NotImplementedException();
        }

        public bool ExistsFile(string path)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsFileAsync(string path)
        {
            throw new NotImplementedException();
        }

        public IDirectory GetDirectory(string path)
        {
            throw new NotImplementedException();
        }

        public Task<IDirectory> GetDirectoryAsync(string path)
        {
            throw new NotImplementedException();
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