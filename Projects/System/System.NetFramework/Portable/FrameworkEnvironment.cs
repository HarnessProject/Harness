using System;
using System.Collections.Generic;
using System.Composition.Providers;
using System.Linq;
using System.Portable.IO;
using System.Text;
using System.Threading.Tasks;

namespace System.Portable
{
    public class FrameworkEnvironment : IEnvironment
    {
        public IDirectory BaseDirectory { get { return new RuntimeDirectory {Path = AppDomain.CurrentDomain.BaseDirectory}; } }
        public IDirectory Settings { get; private set; }
        public IDirectory Documents { get; private set; }
        public IDirectory Downloads { get; private set; }
        public IDirectory Music { get; private set; }
        public IDirectory Pictures { get; private set; }
        public IDirectory Videos { get; private set; }
    }
}
