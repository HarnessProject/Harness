using System;
using System.IO;
using System.Portable.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace System.Portable.Tests
{
    [TestClass]
    public class RuntimeFileSystemTest
    {
        public string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
        [TestMethod]
        public void ConvertStringToRuntimeDirectory()
        {
            var directory = BaseDir.As<RuntimeDirectory>();
            Assert.IsNotNull(directory);
        }

        [TestMethod]
        public void ConvertDirectoryInfoToRuntimeDirectory()
        {
            var directory = new DirectoryInfo(BaseDir);
            Assert.IsNotNull(directory.As<RuntimeDirectory>());
        }
    }
}
