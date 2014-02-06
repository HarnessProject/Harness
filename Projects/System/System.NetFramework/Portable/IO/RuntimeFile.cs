using System.IO;
using System.Threading.Tasks;

namespace System.Portable.IO
{
    public class RuntimeFile : RuntimeFileSystemElement, IFile
    {
        public long Length { get; private set; }
        public Stream Open(FileAccessType fileAccess)
        {
            Stream r = null;
            switch (fileAccess)
            {
                case FileAccessType.Read:
                    r = File.Open(Path, FileMode.Open, System.IO.FileAccess.Read);
                    break;
                case FileAccessType.ReadWrite:
                    r = File.Open(Path, FileMode.OpenOrCreate, System.IO.FileAccess.ReadWrite);
                    break;
                case FileAccessType.Write:
                    r = File.Open(Path, FileMode.OpenOrCreate, System.IO.FileAccess.Write);
                    break;
                default:
                    r = null;
                    break;
            }
            return r;
        }

        public Task<Stream> OpenAsync(FileAccessType fileAccess)
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

        public static implicit operator RuntimeFile(FileInfo file)
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

        public static implicit operator RuntimeFile(string file) {
            return new FileInfo(file).As<RuntimeFile>();
        }
    }
}