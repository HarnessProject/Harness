using System.Collections.Generic;

namespace Harness.Storage {
    public interface IFileSystem {
        bool TryGetFileInfo(string subpath, out IFileInfo fileInfo);
        bool TryGetDirectoryContents(string subpath, out IEnumerable<IFileInfo> contents);
        string MapPath(string subpath);
    }
}