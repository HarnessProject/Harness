namespace System.Portable.IO
{
    public class RuntimeFileSystemElement : IFileSystemElement
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool Exists { get; set; }
    }
}