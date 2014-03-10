using RuntimePath = System.IO.Path;

namespace System.Portable.IO
{
    public abstract class RuntimeFileSystemElement : IFileSystemElement
    {
        public string Name
        {
            get { return System.IO.Path.GetFileName(Path); }
        }

        public string Path { get; set; }
        public abstract bool Exists { get; }
    }
}