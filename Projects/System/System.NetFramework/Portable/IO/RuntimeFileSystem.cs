using System;
using System.IO;
using System.Threading.Tasks;

namespace System.Portable.IO
{
    public class RuntimeFileSystem : IFileSystem
    {
        public IDirectory AppDirectory { get { return AppDomain.CurrentDomain.BaseDirectory.As<RuntimeDirectory>(); } }

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