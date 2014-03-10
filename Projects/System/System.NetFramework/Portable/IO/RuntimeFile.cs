using System.IO;
using System.Threading.Tasks;

namespace System.Portable.IO
{
    public class RuntimeFile : RuntimeFileSystemElement, IFile
    {
        public long Length { get { return new FileInfo(Path).Length; } }
        public Stream Open(FileAccessType fileAccess)
        {
            Stream r = null;
            switch (fileAccess)
            {
                case FileAccessType.Read:
                    r = File.Open(Path, FileMode.Open, FileAccess.Read);
                    break;
                case FileAccessType.ReadWrite:
                    r = File.Open(Path, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    break;
                case FileAccessType.Write:
                    r = File.Open(Path, FileMode.OpenOrCreate, FileAccess.Write);
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

        public static implicit operator RuntimeFile(FileInfo file) {
            return new RuntimeFile {Path = file.FullName};
        }

        public static implicit operator RuntimeFile(string file) {
            return new FileInfo(file).As<RuntimeFile>();
        }

        public override bool Exists { get { return File.Exists(Path); } }
    }
}