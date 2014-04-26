using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Harness.Framework.Interfaces;
using Harness.Framework.Tasks;
using PCLStorage;

namespace Harness.Framework.Net
{
    public class FileSystemLocations : IFileSystemLocations
    {
        public IFolder BaseDirectory
        {
            get {
                return FileSystem.Current.GetFolderFromPathAsync(AppDomain.CurrentDomain.BaseDirectory).AwaitResult();
            }
        }

        public IFolder LocalStorage
        {
            get {
                return FileSystem.Current.LocalStorage;
            }
        }

        public IFolder RoamingStorage
        {
            get {
                return FileSystem.Current.RoamingStorage;
            }
        }
    }
}
