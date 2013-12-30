using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Portable.IO;
using System.Text;

namespace System
{
    public static class UriExtensions
    {
        public static bool IsFile(this Uri uri) {
            return Path.KnownRoots.Any(x => uri.OriginalString.IsMatch(x));
        }
    }
}
