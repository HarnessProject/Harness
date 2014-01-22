#region ApacheLicense

// System.Portable.Base
// Copyright © 2013 Nick Daniels et all, All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License") with the following exception:
// 	Some source code is licensed under compatible licenses as required.
// 	See the attribution headers of the applicable source files for specific licensing 	terms.
// 
// You may not use this file except in compliance with its License(s).
// 
// You may obtain a copy of the Apache License, Version 2.0 at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 

#endregion

namespace System.Portable.IO {
    public enum PathType {
        Unknown = 0,
        Windows = 1,
        Unix = 2,
        Unc = 3,
        Url = 4
    }

    public static class FilePath {
        public static char[] KnownSeperators { get { return new[] {'/', '\\'}; } }

        /// <summary>
        ///     Gets the Regex Pattern of known roots for paths...
        /// </summary>
        /// <value>
        ///     Patterns for known path roots.
        /// </value>
        public static string[] KnownRoots {
            get { return new[] {@"([\w]\:[.*])", @"\/([.*])", @"\\\\([^\\*])\([.*])", @"([\w*])://([.*])"}; // c:\path , /path, \\server\path, proto://path
            }
        }

        public static string GetPattern(PathType t) {
            return t == PathType.Unknown ? "" : KnownRoots[t.As<int>()];
        }

        public static bool IsWindowsPath(string path) {
            return path.IsMatch(GetPattern(PathType.Windows));
        }

        public static bool IsUnixPath(string path) {
            return path.IsMatch(GetPattern(PathType.Unix));
        }
    }
}