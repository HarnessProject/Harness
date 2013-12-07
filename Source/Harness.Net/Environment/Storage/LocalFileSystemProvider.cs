using System;
using System.Collections.Generic;
using System.IO;
using Harness.Storage;

namespace Harness.Environment.Storage
{
    public class LocalFileSystem : IFileSystem {
        public IFileSystem FileSystem { get; set; }
        protected string BasePath = AppDomain.CurrentDomain.BaseDirectory;
        public LocalFileSystem() {}

        //Asp.Net Style.. ~/dir/file.name
        public bool TryGetFileInfo(string subpath, out IFileInfo fileInfo) {
            throw new NotImplementedException();
        }

        public bool TryGetDirectoryContents(string subpath, out IEnumerable<IFileInfo> contents) {
            throw new NotImplementedException();
        }

        public string MapPath(string subpath) {
            var path =  
                subpath
                .Replace("~", BasePath)
                .Replace('/', Path.DirectorySeparatorChar); //Even if it's the same one!

        }

        public bool PathExists(string subpath) {
            var path = MapPath(subpath);
            return Directory.Exists(path);
        }

        public bool FileExists(string filepath) {
            var path = MapPath(filepath);
            return File.Exists(path);
        }

        public void Initialize() {
            FileSystem = new PhysicalFileSystem(BasePath);
            
        }

    }
}