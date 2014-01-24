using System;
using System.IO;
using System.Portable.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Portable.Tests
{
    [TestClass]
    public class RuntimeFileSystemTests
    {
        public string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
        
        [TestMethod]
        public void ConvertDirectoryInfoToRuntimeDirectory()
        {
            var directory = new DirectoryInfo(BaseDir);
            Assert.IsNotNull(directory.As<RuntimeDirectory>());
            Assert.IsTrue(directory.Exists);
        }
        
        [TestMethod]
        public void ConvertStringToRuntimeDirectory()
        {
            var directory = BaseDir.As<RuntimeDirectory>();
            Assert.IsNotNull(directory);
            Assert.IsTrue(directory.Exists);
        }

        [TestMethod]
        public void ConvertFileInfoToRuntimeFile()
        {
            var file = new FileInfo(Path.Combine(BaseDir, "System.Portable.Tests.dll"));
            Assert.IsNotNull(file.As<RuntimeFile>());
            Assert.IsTrue(file.Exists);
        }
        
        
    }
}
