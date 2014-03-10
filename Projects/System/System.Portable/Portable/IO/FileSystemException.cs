#region ApacheLicense

// From the Harness Project
// System.Portable
// Copyright © 2014 Nick Daniels et all, All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License") with the following exception:
// 	Some source code is licensed under compatible licenses as required.
// 	See the attribution headers of the applicable source files for specific licensing terms.
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

#region

using System.IO;

#endregion

namespace System.Portable.IO {

    public enum FileSystemExceptionType { NotFound, ReadOnly, Exists }
    public enum FileSystemExceptionTarget { FileSystem, File, Directory }

    public class FileSystemException : Exception {
        public FileSystemException() {}

        public FileSystemException(string message, FileSystemExceptionTarget target, FileSystemExceptionType type) : base(message) {
            Target = target;
            Type = type;
        }

        public FileSystemExceptionTarget Target { get; set; }
        public FileSystemExceptionType Type { get; set; }

        public static FileSystemException DirectoryNotFound(string path) {
            return
                new FileSystemException(
                    "The Directory specified does not exist : {0}".WithParams(path),
                    FileSystemExceptionTarget.Directory,
                    FileSystemExceptionType.NotFound
                    );
        }

        public static FileSystemException FileExists(string path) {
            return
                new FileSystemException("The File exists : {0}".WithParams(path), FileSystemExceptionTarget.File, FileSystemExceptionType.Exists);
        }

        public static FileSystemException DirectoryExists(string path) {
            return
                new FileSystemException("The Directory exists : {0}".WithParams(path), FileSystemExceptionTarget.Directory, FileSystemExceptionType.Exists );
        }

        public static FileNotFoundException FileNotFound(string path) {
            return
                new FileNotFoundException("The File does not exist : {0}".WithParams(path));
        }
    }
}