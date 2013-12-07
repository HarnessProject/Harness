using System;
using System.Collections.Generic;
using System.Linq;
using Harness.Framework;

namespace Harness.Storage
{
    
    public class CompositeFileSystem : IFileSystem
    {
        protected IEnumerable<IFileSystemProvider> Providers { get; set; }
        
        public CompositeFileSystem(IEnumerable<IFileSystemProvider> providers)
        {
            Providers = providers;
            Providers.Each(x => x.Initialize());
        }

        public string MapPath(string subpath) {
            var paths = Providers.Select(x => new Tuple<IFileSystemProvider, string>(x, x.MapPath(subpath))).ToArray();
            var path = paths.FirstOrDefault(x => x.Item1.PathExists(x.Item2));
            return path != null ? path.Item2 : null;
        }

        public bool TryGetFileInfo(string subpath, out IFileInfo fileInfo) {
            IFileInfo result = null;
            Providers.Each(x => {
                if (result == null)
                    x.FileSystem.TryGetFileInfo(subpath, out result);
            });
            fileInfo = result;
            return result != null;
        }

        public bool TryGetDirectoryContents(string subpath, out IEnumerable<IFileInfo> contents) {
            var files = new List<IFileInfo>();
            Providers.Each(x => {
                IEnumerable<IFileInfo> fs = new List<IFileInfo>();
                
                x.FileSystem.TryGetDirectoryContents(subpath, out fs);
                
                var fileInfos = fs as IFileInfo[] ?? fs.ToArray();
                if (fileInfos.Length > 0) files.AddRange(fileInfos);
            });
            contents = files;
            return files.Any();
        }
    }
}
