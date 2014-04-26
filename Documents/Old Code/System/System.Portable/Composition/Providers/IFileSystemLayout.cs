using System.Composition.IO;

namespace System.Composition.Providers {
    public interface IFileSystemLayout {
        IDirectory BaseDirectory { get; }
        IDirectory Settings { get; }
        IDirectory Documents { get; }
        IDirectory Downloads { get; }
        IDirectory Music { get; }
        IDirectory Pictures { get; }
        IDirectory Videos { get; }
    }
}