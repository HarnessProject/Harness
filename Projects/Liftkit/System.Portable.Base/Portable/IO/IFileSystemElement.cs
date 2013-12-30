namespace System.Portable.IO {
    public interface IFileSystemElement
    {
        string Name { get; }
        string Path { get; }
        bool Exists { get; }
    }
}